namespace JsServer.Packets;

public class ByteStream
{
    private int index = 0;
    private byte[] buffer;
    
    public ByteStream(byte[] bytes)
    {
        buffer = bytes;
    }

    public bool HasNext()
    {
        return index < buffer.Length;
    }

    public bool TryRead(out byte value)
    {
        value = 0;
        if (index >= buffer.Length) return false;
        value = Next();
        return true;
    }

    public byte Next()
    {
        return buffer[index++];
    }

    public byte[] Nexts(int count)
    {
        byte[] bytes = new byte[count];

        for (int i = 0; i < count; i++)
        {
            bytes[i] = Next();
        }

        return bytes;
    }
}