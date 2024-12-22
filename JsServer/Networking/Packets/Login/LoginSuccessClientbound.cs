namespace JsServer.Packets.Login;

public class LoginSuccessClientbound : Packet
{
    private Guid uid;
    private String username;
    private ProfileProperty[] properties;
    public LoginSuccessClientbound(Guid uid, String username, ProfileProperty[] properties)
    {
        this.uid = uid;
        this.username = username;
        this.properties = properties;
    }
    public override void process(Connection connection)
    {
        throw new OperationCanceledException("Attempted to process a clientbound packet");
    }

    public override byte[] convert()
    {
        PacketOutputStream packetOutputStream = new PacketOutputStream();
        packetOutputStream.WriteUUID(uid)
            .WriteString(username)
            .WriteVarInt(properties.Length);
        foreach(ProfileProperty p in properties)
        {
            bool signed = p.HasSignature(out var s);
            packetOutputStream.WriteString(p.GetName())
                .WriteString(p.GetValue())
                .WriteBoolean(signed);
            if (signed)
            {
                packetOutputStream.WriteString(s);
            }
        }

        return packetOutputStream.ToArray(0x02);
    }
}