using System.Text;

namespace JsServer.Packets;

public class PacketOutputStream
{
    private static readonly int VARINT_SEGMENT_BITS = 0x7F;
    private static readonly int VARINT_CONTINUE_BIT = 0x80;
    
    Queue<byte> _queue = new();
    public PacketOutputStream()
    {
        
    }

    public byte[] ToArray(int id)
    {
        byte[] idBytes = encodeVarInt(id);
        byte[] length = encodeVarInt(_queue.Count+idBytes.Length);
        byte[] byteArray = new byte[_queue.Count+idBytes.Length+length.Length];
        int index = 0;
        foreach (byte b in length)
        {
            byteArray[index++] = b;
        }
        foreach (byte b in idBytes)
        {
            byteArray[index++] = b;
        }
        foreach (byte b in _queue)
        {
            byteArray[index++] = b;
        }
        return byteArray;
    }

    private byte[] encodeVarInt(int value)
    {
        List<byte> byteList = new List<byte>();
        int start = 0;
        while (true)
        {
            if ((value & ~VARINT_SEGMENT_BITS) == 0)
            {
                byteList.Add((byte)value);
                return byteList.ToArray();
            }
            
            byteList.Add((byte)((value & VARINT_SEGMENT_BITS) | VARINT_CONTINUE_BIT));

            value >>>= 7;
        }
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
            
            _queue.Enqueue((byte)((value & VARINT_SEGMENT_BITS) | VARINT_CONTINUE_BIT));

            value >>>= 7;
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

    public PacketOutputStream WriteUUID(Guid uid)
    {
        WriteBytes(uid.ToByteArray());
        return this;
    }
}