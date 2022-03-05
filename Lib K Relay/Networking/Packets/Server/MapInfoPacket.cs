namespace Lib_K_Relay.Networking.Packets.Server
{
    public class MapInfoPacket : Packet
    {
        public bool AllowPlayerTeleport;
        public int Background;
        public int BuildVersion;
        public float Difficulty;
        public string DisplayName;
        public string[] DungeonModifiers;
        public uint GameOpenedTime;
        public int Height;
        public short MaxPlayers;
        public string Name;
        public string RealmName;
        public uint Seed;
        public bool ShowDisplays;
        public bool UnknownBool;
        public int UnknownInt;
        public int Width;


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
            Difficulty = r.ReadSingle();
            AllowPlayerTeleport = r.ReadBoolean();
            ShowDisplays = r.ReadBoolean();
            MaxPlayers = r.ReadInt16();
            GameOpenedTime = r.ReadUInt32();
            BuildVersion = r.ReadInt32();
            UnknownBool = r.ReadBoolean();
            UnknownInt = r.ReadInt32();
            DungeonModifiers = r.ReadString().Split(',');
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
            w.Write(UnknownBool);
            w.Write(UnknownInt);
            w.Write(string.Join(",", DungeonModifiers));
        }
    }
}