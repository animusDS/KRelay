namespace Lib_K_Relay.Networking.Packets.Server
{
    public class MapInfoPacket : Packet
    {
        public bool AllowPlayerTeleport;
        public int Background;
        public string BuildVersion;
        public int Difficulty;
        public string DisplayName;
        public uint GameOpenedTime;
        public int Height;
        public int Width;
        public short MaxPlayers;
        public string Name;
        public string RealmName;
        public uint Seed;
        public bool ShowDisplays;
        public string[] DungeonModifiers;
        public bool UnknownBool;
        public int UnknownInt;


        public override PacketType Type => PacketType.MAPINFO;

        public override void Read(PacketReader r)
        {
            Width = r.ReadInt32();
            Height = r.ReadInt32();
            Name = r.ReadString();
            DisplayName = r.ReadString();
            RealmName = r.ReadString();
            Seed = r.ReadUInt32();
            Background = r.ReadInt32();
            Difficulty = r.ReadInt32();
            AllowPlayerTeleport = r.ReadBoolean();
            ShowDisplays = r.ReadBoolean();
            MaxPlayers = r.ReadInt16();
            GameOpenedTime = r.ReadUInt32();
            BuildVersion = r.ReadString();
            DungeonModifiers = r.ReadString().Split(';');
            UnknownBool = r.ReadBoolean();
            UnknownInt = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Width);
            w.Write(Height);
            w.Write(Name);
            w.Write(DisplayName);
            w.Write(RealmName);
            w.Write(Seed);
            w.Write(Background);
            w.Write(Difficulty);
            w.Write(AllowPlayerTeleport);
            w.Write(ShowDisplays);
            w.Write(MaxPlayers);
            w.Write(GameOpenedTime);
            w.Write(BuildVersion);
            w.Write(string.Join(";", DungeonModifiers));
            w.Write(UnknownBool);
            w.Write(UnknownInt);
        }
    }
}