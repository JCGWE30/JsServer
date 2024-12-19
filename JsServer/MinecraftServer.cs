namespace JsServer;

public class MinecraftServer
{
    public static readonly int PROTOCOL_VERSION = 769;
    
    private PacketListener _packetListener;
    public void start()
    {
        _packetListener = new PacketListener();

        while (true)
        {
            _packetListener.listenerTick();
        }
    }
}