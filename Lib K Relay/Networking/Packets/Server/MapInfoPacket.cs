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
        public short MaxPlayers;
        public string Name;
        public string RealmName;
        public uint Seed;
        public bool ShowDisplays;
        public byte Unknown;
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
            Difficulty = r.ReadInt32();
            AllowPlayerTeleport = r.ReadBoolean();
            ShowDisplays = r.ReadBoolean();
            MaxPlayers = r.ReadInt16();
            GameOpenedTime = r.ReadUInt32();
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
        }
    }
}