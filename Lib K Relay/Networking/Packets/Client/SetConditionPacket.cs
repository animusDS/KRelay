namespace Lib_K_Relay.Networking.Packets.Client
{
    public class SetConditionPacket : Packet
    {
        public float ConditionDuration;
        public byte ConditionEffect;

        public override PacketType Type => PacketType.SETCONDITION;

        public override void Read(PacketReader r)
        {
            ConditionEffect = r.ReadByte();
            ConditionDuration = r.ReadSingle();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(ConditionEffect);
            w.Write(ConditionDuration);
        }
    }
}