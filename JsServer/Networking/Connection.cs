using System.Net.Sockets;
using System.Security.Cryptography;
using JsServer.Packets;
using JsServer.Packets.Login;

namespace JsServer;

public class Connection
{
    private static readonly long TIMEOUTMS = (long) TimeSpan.FromSeconds(5).TotalMilliseconds;
    
    public bool shouldDrop => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - lastActivity > TIMEOUTMS;
    public int state;
    
    private long lastActivity;
    private TcpClient client;

    public byte[] verifyBytes;
    public String name;
    public Guid guid;
    public Aes? aes;
    
    public Connection(TcpClient client)
    {
        this.client = client;
        state = 1;
        lastActivity = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    public void processBytes(byte[] bytes)
    {
        PacketStream stream = new PacketStream(bytes);
        while (stream.HasNext())
        {
            int length = stream.ReadVarInt();
            if(length==0) return;
            Packet p = Packet.CreatePacket(stream.Nexts(length),state);
            p.process(this);
        }
    }

    public void terminate()
    {
        //Nuffin for now
    }

    public void SendPacket(Packet packet)
    {
        byte[] goingBytes = packet.convert();
        if (aes != null)
        {
            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                using (var ms = new MemoryStream())
                using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(goingBytes, 0, goingBytes.Length);
                    cryptoStream.FlushFinalBlock();

                    goingBytes = ms.ToArray();
                }
            }
        }
        Console.WriteLine(String.Join(" ", goingBytes));
        client.GetStream().Write(goingBytes, 0, goingBytes.Length);
    }
}