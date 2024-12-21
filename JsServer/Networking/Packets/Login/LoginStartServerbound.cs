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
        Console.WriteLine($"Logging in as {name} [{uuid.ToString()}]");
        EncryptionRequestClientbound encryptionPacket =
            new EncryptionRequestClientbound("                    ", MinecraftServer.publicKey);
        connection.SendPacket(encryptionPacket);
    }
    
    public override byte[] convert()
    {
        throw new InvalidOperationException("Attempted to seralize a serverbound packet");
    }
}