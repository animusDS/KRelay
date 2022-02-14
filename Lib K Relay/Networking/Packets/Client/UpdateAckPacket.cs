namespace Lib_K_Relay.Networking.Packets.Client
{
    public class UpdateAckPacket : Packet
    {
        public override PacketType Type => PacketType.UPDATEACK;

        public override void Read(PacketReader r)
        {
        }

        public override void Write(PacketWriter w)
        {
        }
    }
}