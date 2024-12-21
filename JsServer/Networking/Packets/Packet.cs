namespace JsServer.Packets;

public abstract class Packet
{
    public abstract void process(Connection connection);
    
    public abstract byte[] convert();
    
    public static Packet CreatePacket(byte[] data,int state,bool isOutgoing = false)
    {
        PacketStream stream = new PacketStream(data);
        int packetId = stream.ReadVarInt();
        Packet packet = (Packet) Activator.CreateInstance(PacketTypeManager.Get(packetId,state,isOutgoing),stream);
        return packet;
    }
}