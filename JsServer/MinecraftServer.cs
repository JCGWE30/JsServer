using System.Security.Cryptography;

namespace JsServer;

public class MinecraftServer
{
    public static readonly int PROTOCOL_VERSION = 769;
    
    private static MinecraftServer _instance;

    public static byte[] publicKey => _instance._rsa.ExportSubjectPublicKeyInfo();
    public static RSA serverRsa => _instance._rsa;

    private PacketListener _packetListener;
    private RSA _rsa;
    public void start()
    {
        _instance = this;
        _packetListener = new PacketListener();
        _rsa = RSA.Create(1024);

        while (true)
        {
            _packetListener.listenerTick();
        }
    }
}