namespace Lib_K_Relay.Networking.Packets.Server
{
    public class ImminentArenaWave : Packet
    {
        public int Time;

        public override PacketType Type => PacketType.IMMINENT_ARENA_WAVE;

        public override void Read(PacketReader r)
        {
            Time = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Time);
        }
    }
}