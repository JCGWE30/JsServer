namespace JsServer.Packets.Login;

public class EncryptionRequestClientbound : Packet
{
    private String serverId;
    private byte[] publicKey;
    private byte[] verificationBytes;

    public EncryptionRequestClientbound(String serverId, byte[] publicKey,byte[] verificationBytes)
    {
        this.serverId = serverId;
        this.publicKey = publicKey;
        this.verificationBytes = verificationBytes;
    }
    public override void process(Connection connection)
    {
        throw new OperationCanceledException("Attempted to process a clientbound packet");
    }

    public override byte[] convert()
    {
        PacketOutputStream stream = new PacketOutputStream();
        stream.WriteString(serverId);
        stream.WriteVarInt(publicKey.Length);
        stream.WriteBytes(publicKey);
        stream.WriteVarInt(verificationBytes.Length);
        stream.WriteBytes(verificationBytes);
        stream.WriteBoolean(true);
        return stream.ToArray(0x01);
    }
}