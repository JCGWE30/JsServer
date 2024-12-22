using System.Numerics;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Aes = System.Security.Cryptography.Aes;

namespace JsServer.Packets.Login;

public class EncryptionResponseServerbound : Packet
{
    private byte[] sharedSecret;
    private byte[] verifyToken;
    public EncryptionResponseServerbound(PacketStream stream)
    {
        int secretLength = stream.ReadVarInt();
        sharedSecret = MinecraftServer.serverRsa.Decrypt(stream.ReadBytes(secretLength),RSAEncryptionPadding.Pkcs1);
        int verifyLength = stream.ReadVarInt();
        verifyToken = MinecraftServer.serverRsa.Decrypt(stream.ReadBytes(verifyLength),RSAEncryptionPadding.Pkcs1);
        Console.WriteLine("All Done");
    }
    public override void process(Connection connection)
    {
        String hash;
        using (SHA1 sha1 = SHA1.Create())
        {
            byte[] serverIdBytes = " "u8.ToArray();

            sha1.TransformBlock(serverIdBytes, 0, serverIdBytes.Length, null, 0);
            sha1.TransformBlock(sharedSecret, 0, sharedSecret.Length, null, 0);
            
            sha1.TransformFinalBlock(MinecraftServer.publicKey, 0, MinecraftServer.publicKey.Length);
            
            hash = mcDigest(sha1);
        }

        String url =
            "https://sessionserver.mojang.com/session/minecraft/hasJoined?username=%username%&serverId=%hash%"
                .Replace("%username%", "LePigSlayer")
                .Replace("%hash%", hash);

        HttpClient client = new HttpClient();
        try
        {
            Console.WriteLine(url);
            Task.Run(async () => await client.GetStringAsync(url))
                .ContinueWith(t =>
                    { 
                        List<ProfileProperty> properties = new List<ProfileProperty>();
                        JsonNode p = JsonObject.Parse(t.Result);
                        foreach (JsonNode n in p["properties"].AsArray())
                        {
                            properties.Add(new ProfileProperty(n["name"].ToString(),n["value"].ToString(),n["signature"]?.ToString()));
                        }
                        string id = p["id"].ToString();
                        StringBuilder uid = new StringBuilder();
                        uid.Append(id.Substring(0, 8));
                        uid.Append("-");
                        uid.Append(id.Substring(8, 4));
                        uid.Append("-");
                        uid.Append(id.Substring(12, 4));
                        uid.Append("-");
                        uid.Append(id.Substring(16,4));
                        uid.Append("-");
                        uid.Append(id.Substring(20));
                        LoginSuccessClientbound packet = new LoginSuccessClientbound(
                            Guid.Parse(uid.ToString()),
                            p["name"].ToString(),
                            properties.ToArray());
                        Aes aes = Aes.Create();
                        aes.Mode = CipherMode.CFB;
                        aes.Padding = PaddingMode.None;
                        aes.FeedbackSize = 8;
                        aes.Key = sharedSecret;
                        aes.IV = sharedSecret;
                        connection.aes = aes;
                        connection.SendPacket(packet);
                    }
                );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public override byte[] convert()
    {
        throw new InvalidOperationException("Attempted to seralize a serverbound packet");
    }

    private String mcDigest(SHA1 sha1)
    {
        byte[] hash = sha1.Hash;
        Array.Reverse(hash);
        
        BigInteger b = new BigInteger(hash);

        if (b < 0)
        {
            return "-" + (-b).ToString("x").TrimStart('0');
        }
        else
        {
            return b.ToString("x").TrimStart('0');
        }
    }
}