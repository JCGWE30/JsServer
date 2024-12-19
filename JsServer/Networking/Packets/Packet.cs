namespace JsServer.Packets;

public abstract class Packet
{
    private PacketStream _stream;
    public Packet(PacketStream stream)
    {
        this._stream = stream;
    }
    
    public abstract void process(Connection connection);
    
    public static Packet CreatePacket(byte[] data,int state,bool isOutgoing = false)
    {
        PacketStream stream = new PacketStream(data);
        int packetId = stream.ReadVarInt();
        Packet packet = (Packet) Activator.CreateInstance(PacketTypeManager.Get(packetId,state,isOutgoing),stream);
        return packet;
    }
}