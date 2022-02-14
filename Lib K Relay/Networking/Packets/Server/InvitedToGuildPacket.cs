namespace Lib_K_Relay.Networking.Packets.Server
{
    public class InvitedToGuildPacket : Packet
    {
        public string GuildName;
        public string Name;

        public override PacketType Type => PacketType.INVITEDTOGUILD;

        public override void Read(PacketReader r)
        {
            Name = r.ReadString();
            GuildName = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Name);
            w.Write(GuildName);
        }
    }
}