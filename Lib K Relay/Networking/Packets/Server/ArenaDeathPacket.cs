namespace Lib_K_Relay.Networking.Packets.Server
{
    public class ArenaDeathPacket : Packet
    {
        public int RestartPrice;

        public override PacketType Type => PacketType.ARENA_DEATH;

        public override void Read(PacketReader r)
        {
            RestartPrice = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(RestartPrice);
        }
    }
}