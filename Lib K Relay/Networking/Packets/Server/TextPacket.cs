namespace Lib_K_Relay.Networking.Packets.Server
{
    public class TextPacket : Packet
    {
        public int BubbleTime;
        public string CleanText = "";
        public bool IsSupporter;
        public string Name = "";
        public short NumStars;
        public int ObjectId;
        public string Recipient = "";
        public int StarBackground;
        public string Text = "";

        public override PacketType Type => PacketType.TEXT;

        public override void Read(PacketReader r)
        {
            Name = r.ReadString();
            ObjectId = r.ReadInt32();
            NumStars = r.ReadInt16();
            BubbleTime = r.ReadByte();
            Recipient = r.ReadString();
            Text = r.ReadString();
            CleanText = r.ReadString();
            IsSupporter = r.ReadBoolean();
            StarBackground = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Name);
            w.Write(ObjectId);
            w.Write(NumStars);
            w.Write(BubbleTime);
            w.Write(Recipient);
            w.Write(Text);
            w.Write(CleanText);
            w.Write(IsSupporter);
            w.Write(StarBackground);
        }
    }
}