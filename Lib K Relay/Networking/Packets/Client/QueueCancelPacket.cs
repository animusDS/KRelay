namespace Lib_K_Relay.Networking.Packets.Client
{
    public class QueueCancelPacket : Packet
    {
        public int ObjectId;

        public override PacketType Type => PacketType.QUEUE_CANCEL;

        public override void Read(PacketReader r)
        {
            ObjectId = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(ObjectId);
        }
    }
}