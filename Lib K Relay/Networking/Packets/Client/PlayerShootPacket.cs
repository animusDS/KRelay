using Lib_K_Relay.Networking.Packets.DataObjects.Location;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class PlayerShootPacket : Packet
    {
        public float Angle;
        public short BulletId;
        public byte BurstId;
        public ushort ContainerType;
        public Location Position;
        public int Time;

        public override PacketType Type => PacketType.PLAYERSHOOT;

        public override void Read(PacketReader r)
        {
            Time = r.ReadInt32();
            BulletId = r.ReadInt16();
            ContainerType = r.ReadUInt16();
            Position = (Location)new Location().Read(r);
            Angle = r.ReadSingle();
            BurstId = r.ReadByte();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Time);
            w.Write(BulletId);
            w.Write(ContainerType);
            Position.Write(w);
            w.Write(Angle);
            w.Write(BurstId);
        }
    }
}