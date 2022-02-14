namespace Lib_K_Relay.Networking.Packets.Server
{
    public class DeathPacket : Packet
    {
        public string AccountId;
        public int CharId;
        public string KilledBy;

        public override PacketType Type => PacketType.DEATH;

        public override void Read(PacketReader r)
        {
            AccountId = r.ReadString();
            CharId = r.ReadInt32();
            KilledBy = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(AccountId);
            w.Write(CharId);
            w.Write(KilledBy);
        }
    }
}