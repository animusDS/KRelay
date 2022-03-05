namespace Lib_K_Relay.Networking.Packets.Server
{
    public class NotificationPacket : Packet
    {
        public int Color;
        public byte Effect;
        public byte Extra;

        public string Message;
        public int ObjectId;
        public int PictureType;
        public int QueuePosition;
        public int UiExtra;

        public int Unknown32;
        public byte UnknownByte;

        public override PacketType Type => PacketType.NOTIFICATION;

        public override void Read(PacketReader r)
        {
            Effect = r.ReadByte();
            Extra = r.ReadByte();
            switch (Effect)
            {
                case 1: // Stat Increase
                    Message = r.ReadString();
                    break;
                case 2: // Server Message
                    Message = r.ReadString();
                    break;
                case 3: // Error Message
                    Message = r.ReadString();
                    break;
                case 4: // UI
                    UiExtra = r.ReadInt16();
                    Message = r.ReadString();
                    break;
                case 5: // Queue
                    Message = r.ReadString();
                    QueuePosition = r.ReadInt32();
                    break;
                case 6: // Object Text / JSON
                    ObjectId = r.ReadInt32();
                    Message = r.ReadString();
                    //Color = r.ReadInt32();
                    break;
                case 7: // Death
                    Message = r.ReadString();
                    PictureType = r.ReadInt32();
                    break;
                case 8: // Dungeon Opened
                    Message = r.ReadString();
                    PictureType = r.ReadInt32();
                    break;
                case 10: // Dungeon Call
                    ObjectId = r.ReadInt32();
                    Unknown32 = r.ReadInt32();
                    UnknownByte = r.ReadByte();
                    break;
            }
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Effect);
            w.Write(Extra);
            switch (Effect)
            {
                case 1: // Stat Increase
                    w.Write(Message);
                    break;
                case 2: // Server Message
                    w.Write(Message);
                    break;
                case 3: // Error Message
                    w.Write(Message);
                    break;
                case 4: // UI
                    w.Write(UiExtra);
                    w.Write(Message);
                    break;
                case 5: // Queue
                    w.Write(ObjectId);
                    w.Write(Message);
                    w.Write(QueuePosition);
                    break;
                case 6: // Object Text / JSON
                    w.Write(ObjectId);
                    w.Write(Message);
                    //w.Write(Color);
                    break;
                case 7: // Death
                    w.Write(Message);
                    w.Write(PictureType);
                    break;
                case 8: // Dungeon Opened
                    w.Write(Message);
                    w.Write(PictureType);
                    break;
                case 10: // Dungeon Call
                    w.Write(ObjectId);
                    w.Write(Unknown32);
                    w.Write(UnknownByte);
                    break;
            }
        }
    }
}