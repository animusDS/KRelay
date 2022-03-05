using Lib_K_Relay.Networking.Packets.DataObjects.Location;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class GotoPacket : Packet
    {
        public Location Location;
        public int ObjectId;
        public int Unknown;

        public override PacketType Type => PacketType.GOTO;

        public override void Read(PacketReader r)
        {
            ObjectId = r.ReadInt32();
            Location = (Location)new Location().Read(r);
            Unknown = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(ObjectId);
            Location.Write(w);
            w.Write(Unknown);
        }
    }
}