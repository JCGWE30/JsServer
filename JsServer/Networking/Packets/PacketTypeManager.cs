using JsServer.Packets.Login;
using JsServer.Packets.State;

namespace JsServer.Packets;

public class PacketTypeManager
{
    private static readonly Dictionary<byte, Type> PacketTypes = new Dictionary<byte, Type>
    {
        { 0b00_01_0000, typeof(HandshakeServerbound) },
        { 0b00_10_0000, typeof(LoginStartServerbound) }
    };

    public static Type Get(int type,int state,bool outgoing)
    {
        byte mask = (byte) (type | (byte) (state << 4));
        if (outgoing) mask |= 0x40;
        return PacketTypes[mask];
    }
}