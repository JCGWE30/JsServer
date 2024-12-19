namespace JsServer;

public class MinecraftServer
{
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