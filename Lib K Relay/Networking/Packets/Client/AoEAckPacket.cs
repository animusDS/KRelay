using Lib_K_Relay.Networking.Packets.DataObjects.Location;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class AoEAckPacket : Packet
    {
        public Location Position;
        public int Time;

        public override PacketType Type => PacketType.AOEACK;

        public override void Read(PacketReader r)
        {
            Time = r.ReadInt32();
            Position = (Location)new Location().Read(r);
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Time);
            Position.Write(w);
        }
    }
}