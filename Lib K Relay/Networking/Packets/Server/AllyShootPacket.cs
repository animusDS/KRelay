namespace Lib_K_Relay.Networking.Packets.Server
{
    public class AllyShootPacket : Packet
    {
        public float Angle;
        public int BulletId;
        public ushort ContainerType;
        public bool IsBard;
        public int OwnerId;

        public override PacketType Type => PacketType.ALLYSHOOT;

        public override void Read(PacketReader r)
        {
            BulletId = r.ReadByte();
            OwnerId = r.ReadInt32();
            ContainerType = r.ReadUInt16();
            Angle = r.ReadSingle();
            IsBard = r.ReadBoolean();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(BulletId);
            w.Write(OwnerId);
            w.Write(ContainerType);
            w.Write(Angle);
            w.Write(IsBard);
        }
    }
}