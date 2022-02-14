using Lib_K_Relay.Networking.Packets.DataObjects;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class ServerPlayerShootPacket : Packet
    {
        public float Angle;
        public ushort BulletId;
        public int ContainerType;
        public short Damage;
        public int OwnerId;
        public byte ShotCount;
        public Location StartingLoc;
        public int Unknown1;
        public int Unknown2;

        public override PacketType Type => PacketType.SERVERPLAYERSHOOT;

        public override void Read(PacketReader r)
        {
            BulletId = r.ReadUInt16();
            OwnerId = r.ReadInt32();
            ContainerType = r.ReadInt32();
            StartingLoc = (Location)new Location().Read(r);
            Angle = r.ReadSingle();
            Damage = r.ReadInt16();
            Unknown1 = r.ReadInt32();
            ShotCount = r.ReadByte();
            if (ShotCount > 0)
                Unknown2 = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(BulletId);
            w.Write(OwnerId);
            w.Write(ContainerType);
            StartingLoc.Write(w);
            w.Write(Angle);
            w.Write(Damage);
            w.Write(Unknown1);
            w.Write(ShotCount);
            if (ShotCount > 0)
                w.Write(Unknown2);
        }
    }
}