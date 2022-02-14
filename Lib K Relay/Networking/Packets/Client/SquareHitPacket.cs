namespace Lib_K_Relay.Networking.Packets.Server
{
    public class SquareHitPacket : Packet
    {
        public short BulletId;
        public int ObjectId;
        public int Time;

        public override PacketType Type => PacketType.SQUAREHIT;

        public override void Read(PacketReader r)
        {
            Time = r.ReadInt32();
            BulletId = r.ReadInt16();
            ObjectId = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Time);
            w.Write(BulletId);
            w.Write(ObjectId);
        }
    }
}