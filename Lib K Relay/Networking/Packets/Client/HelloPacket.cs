namespace Lib_K_Relay.Networking.Packets.Client
{
    public class HelloPacket : Packet
    {
        public string AccessToken;
        public string BuildVersion;
        public string ClientToken;
        public string EntryTag;

        public int GameId;

        //public string GameNet;
        public string GameNetUserId;
        public byte[] Key;

        public int KeyTime;

        //public string MapJSON;
        public string PlatformHash;

        public string PlatformToken;
        //public string PlayPlatform;

        public override PacketType Type => PacketType.HELLO;

        public override void Read(PacketReader r)
        {
            BuildVersion = r.ReadString();
            GameId = r.ReadInt32();
            AccessToken = r.ReadString();
            KeyTime = r.ReadInt32();
            Key = r.ReadBytes(r.ReadInt16());
            //MapJSON = r.ReadUTF32();
            EntryTag = r.ReadString();
            //GameNet = r.ReadString();
            GameNetUserId = r.ReadString();
            //PlayPlatform = r.ReadString();
            PlatformToken = r.ReadString();
            ClientToken = r.ReadString();
            PlatformHash = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(BuildVersion);
            w.Write(GameId);
            w.Write(AccessToken);
            w.Write(KeyTime);
            w.Write((short)Key.Length);
            w.Write(Key);
            //w.WriteUTF32(MapJSON);
            w.Write(EntryTag);
            //w.Write(GameNet);
            w.Write(GameNetUserId);
            //w.Write(PlayPlatform);
            w.Write(PlatformToken);
            w.Write(ClientToken);
            w.Write(PlatformHash);
        }
    }
}