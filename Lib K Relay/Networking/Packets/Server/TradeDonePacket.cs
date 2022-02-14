namespace Lib_K_Relay.Networking.Packets.Server
{
    public class TradeDonePacket : Packet
    {
        public string Message;
        /*
		TradeSuccessful = 0
		PlayerCanceled = 1
		*/

        public int Result;

        public override PacketType Type => PacketType.TRADEDONE;

        public override void Read(PacketReader r)
        {
            Result = r.ReadInt32();
            Message = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Result);
            w.Write(Message);
        }
    }
}