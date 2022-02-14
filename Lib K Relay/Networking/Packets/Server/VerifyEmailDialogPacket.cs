namespace Lib_K_Relay.Networking.Packets.Server
{
    public class VerifyEmailDialogPacket : Packet
    {
        public override PacketType Type => PacketType.VERIFY_EMAIL;

        public override void Read(PacketReader r)
        {
        }

        public override void Write(PacketWriter w)
        {
        }
    }
}