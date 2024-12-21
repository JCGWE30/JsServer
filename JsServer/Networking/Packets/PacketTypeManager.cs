using JsServer.Packets.Login;
using JsServer.Packets.State;

namespace JsServer.Packets;

public class PacketTypeManager
{
    /*
     * 00_11_2222
     * 0 - If the packet is server or client bound. 00 - Serverbound, 01 - Clientbound
     * 1 - The state of the packet. 01 - Status, 10 - Login, 11 Play, Transfer Not Supported Yet
     * 2 - The packet ID
     */
    private static readonly Dictionary<byte, Type> PacketTypes = new Dictionary<byte, Type>
    {
        { 0b00_01_0000, typeof(HandshakeServerbound) },
        { 0b00_10_0000, typeof(LoginStartServerbound) },
        { 0b00_10_0001, typeof(EncryptionResponseServerbound) },
        { 0b01_10_0000, typeof(EncryptionRequestClientbound) }
    };

    public static Type Get(int type,int state,bool outgoing)
    {
        byte mask = (byte) (type | (byte) (state << 4));
        if (outgoing) mask |= 0x40;
        return PacketTypes[mask];
    }
}