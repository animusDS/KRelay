namespace Lib_K_Relay.Networking.Packets.Client
{
    public class TeleportPacket : Packet
    {
        public string Name;
        public int ObjectId;

        public override PacketType Type => PacketType.TELEPORT;

        public override void Read(PacketReader r)
        {
            ObjectId = r.ReadInt32();
            Name = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(ObjectId);
            w.Write(Name);
        }
    }
}