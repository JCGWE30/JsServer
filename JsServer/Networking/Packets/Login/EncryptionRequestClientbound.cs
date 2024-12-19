namespace JsServer.Packets.Login;

public class EncryptionRequestClientbound : Packet
{
    public EncryptionRequestClientbound(PacketStream stream) : base(null)
    {
        
    }

    public override void process(Connection connection)
    {
        throw new NotImplementedException();
    }
}