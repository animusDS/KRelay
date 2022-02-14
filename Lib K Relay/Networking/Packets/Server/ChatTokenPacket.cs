namespace Lib_K_Relay.Networking.Packets.Server
{
    public class ChatTokenPacket : Packet
    {
        public string Host;
        public int Port;
        public string Token;

        public override PacketType Type => PacketType.CHAT_TOKEN_MSG;

        public override void Read(PacketReader r)
        {
            Token = r.ReadString();
            Host = r.ReadString();
            Port = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Token);
            w.Write(Host);
            w.Write(Port);
        }
    }
}