namespace Lib_K_Relay.Networking.Packets.Client
{
    public class ChangeGuildRankPacket : Packet
    {
        public byte GuildRank;
        public string Name;

        public override PacketType Type => PacketType.CHANGEGUILDRANK;

        public override void Read(PacketReader r)
        {
            Name = r.ReadString();
            GuildRank = r.ReadByte();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Name);
            w.Write(GuildRank);
        }
    }
}