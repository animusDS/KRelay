namespace Lib_K_Relay.Networking.Packets.Server
{
    public class ExaltationBonusChangedPacket : Packet
    {
        public byte AttackProgress;
        public byte DefenseProgress;
        public byte DexterityProgress;
        public byte HealthProgress;
        public byte ManaProgress;
        public short ObjectType;
        public byte SpeedProgress;
        public byte VitalityProgress;
        public byte WisdomProgress;

        public override PacketType Type => PacketType.EXALTATION_BONUS_CHANGED;

        public override void Read(PacketReader r)
        {
            ObjectType = r.ReadInt16();
            DexterityProgress = r.ReadByte();
            SpeedProgress = r.ReadByte();
            VitalityProgress = r.ReadByte();
            WisdomProgress = r.ReadByte();
            DefenseProgress = r.ReadByte();
            AttackProgress = r.ReadByte();
            ManaProgress = r.ReadByte();
            HealthProgress = r.ReadByte();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(ObjectType);
            w.Write(DexterityProgress);
            w.Write(SpeedProgress);
            w.Write(VitalityProgress);
            w.Write(WisdomProgress);
            w.Write(DefenseProgress);
            w.Write(AttackProgress);
            w.Write(ManaProgress);
            w.Write(HealthProgress);
        }
    }
}