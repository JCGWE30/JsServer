namespace JsServer.Packets.Login;

public class LoginStartServerbound : Packet
{
    private String name;
    private Guid uuid;
    
    public LoginStartServerbound(PacketStream stream)
    {
        name = stream.ReadString();
        uuid = stream.ReadUUID();
    }

    public override void process(Connection connection)
    {
        byte[] verifyBytes = new byte[8];
        new Random().NextBytes(verifyBytes);
        EncryptionRequestClientbound encryptionPacket =
            new EncryptionRequestClientbound(" ", MinecraftServer.publicKey,verifyBytes);
        connection.name = name;
        connection.guid = uuid;
        connection.verifyBytes = verifyBytes;
        connection.SendPacket(encryptionPacket);
    }
    
    public override byte[] convert()
    {
        throw new InvalidOperationException("Attempted to seralize a serverbound packet");
    }
}