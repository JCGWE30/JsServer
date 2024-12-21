using System.Text;

namespace JsServer.Packets;

public class PacketStream : ByteStream
{
    private static readonly int VARINT_SEGMENT_BITS = 0x7F;
    private static readonly int VARINT_CONTINUE_BIT = 0x80;

    public PacketStream(byte[] bytes) : base(bytes)
    {
    }

    public void DropByte()
    {
        Next();
    }

    public int ReadVarInt()
    {
        var value = 0;
        var position = 0;
        byte currentByte;

        while (true)
        {
            currentByte = Next();
            value |= (currentByte & VARINT_SEGMENT_BITS) << position;

            if ((currentByte & VARINT_CONTINUE_BIT) == 0) break;

            position += 7;

            if (position >= 32) throw new OverflowException("VarInt too big");
        }

        return value;
    }

    public string ReadString()
    {
        var length = ReadVarInt();
        var stringBytes = new byte[length];

        for (var i = 0; i < length; i++) stringBytes[i] = Next();

        return Encoding.UTF8.GetString(stringBytes);
    }

    public Guid ReadUUID()
    {
        byte[] uidBytes = Nexts(16);
        
        return new Guid(uidBytes);
    }

    public ushort ReadUShort()
    {
        ushort value = Next();
        value = (ushort)(value << 8);
        value |= Next();
        return value;
    }

    public byte[] ReadBytes(int length)
    {
        byte[] bytes = new byte[length];
        for(int i = 0; i < length; i++) bytes[i] = Next();
        return bytes;
    }
}