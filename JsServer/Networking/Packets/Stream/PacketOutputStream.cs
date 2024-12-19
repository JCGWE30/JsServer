using System.Text;

namespace JsServer.Packets;

public class PacketOutputStream
{
    private static readonly int VARINT_SEGMENT_BITS = 0x7F;
    private static readonly int VARINT_CONTINUE_BIT = 0x80;
    
    Queue<byte> _queue = new Queue<byte>();
    public PacketOutputStream()
    {
        
    }

    public PacketOutputStream WriteVarInt(int value)
    {
        while (true)
        {
            if ((value & ~VARINT_SEGMENT_BITS) == 0)
            {
                _queue.Enqueue((byte)value);
                return this;
            }
            
            _queue.Enqueue((byte)(value & VARINT_SEGMENT_BITS));

            value = value >>> 7;
        }
    }

    public PacketOutputStream WriteString(string str)
    {
        WriteVarInt(str.Length);
        foreach (var b in Encoding.UTF8.GetBytes(str))
        {
            _queue.Enqueue(b);
        }
        return this;
    }

    public PacketOutputStream WriteBytes(byte[] bytes)
    {
        foreach (var b in bytes)
        {
            _queue.Enqueue(b);
        }
        return this;
    }

    public PacketOutputStream WriteBoolean(bool bol)
    {
        _queue.Enqueue(bol ? (byte)1 : (byte)0);
        return this;
    }
}