namespace Lib_K_Relay.Networking.Packets.Client
{
    public class ChatHelloPacket : Packet
    {
        public string AccountId;
        public string Token;

        public override PacketType Type => PacketType.CHAT_HELLO_MSG;

        public override void Read(PacketReader r)
        {
            AccountId = r.ReadString();
            Token = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(AccountId);
            w.Write(Token);
        }
    }
}