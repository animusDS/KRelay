namespace Lib_K_Relay.Networking.Packets.Server
{
    public class ReconnectPacket : Packet
    {
        public int GameId;
        public string Host;
        public byte[] Key;
        public int KeyTime;
        public string Name;
        public ushort Port;

        public override PacketType Type => PacketType.RECONNECT;

        public override void Read(PacketReader r)
        {
            Name = r.ReadString();
            Host = r.ReadString();
            Port = r.ReadUInt16();
            GameId = r.ReadInt32();
            KeyTime = r.ReadInt32();
            Key = r.ReadBytes(r.ReadInt16());
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Name);
            w.Write(Host);
            w.Write(Port);
            w.Write(GameId);
            w.Write(KeyTime);
            w.Write((short)Key.Length);
            w.Write(Key);
        }
    }
}