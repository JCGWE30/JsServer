﻿namespace JsServer.Packets.State;

public class HandshakeServerbound : Packet
{
    private int protocolVersion;
    private string serverAddress;
    private ushort serverPort;
    private int nextState;
    
    public HandshakeServerbound(PacketStream stream)
    {
        protocolVersion = stream.ReadVarInt();
        serverAddress = stream.ReadString();
        serverPort = stream.ReadUShort();
        nextState = stream.ReadVarInt();
    }

    public override void process(Connection connection)
    {
        if (protocolVersion != MinecraftServer.PROTOCOL_VERSION)
        {
            connection.terminate();
            return;
        }
        connection.state = nextState;
    }

    public override byte[] convert()
    {
        throw new InvalidOperationException("Attempted to seralize a serverbound packet");
    }
}