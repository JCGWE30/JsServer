using System.Security.Cryptography;

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
        throw new NotImplementedException();
    }

    public override byte[] convert()
    {
        throw new InvalidOperationException("Attempted to seralize a serverbound packet");
    }
}