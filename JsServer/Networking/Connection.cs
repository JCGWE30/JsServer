using JsServer.Packets;

namespace JsServer;

public class Connection
{
    private static readonly long TIMEOUTMS = (long) TimeSpan.FromSeconds(5).TotalMilliseconds;
    
    public bool shouldDrop => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - lastActivity > TIMEOUTMS;
    public int state;
    
    private long lastActivity;
    
    public Connection()
    {
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
}