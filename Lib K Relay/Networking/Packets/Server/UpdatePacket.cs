using Lib_K_Relay.Networking.Packets.DataObjects.Data;
using Lib_K_Relay.Networking.Packets.DataObjects.Location;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class UpdatePacket : Packet
    {
        public int[] Drops;
        public int LevelType;
        public Entity[] NewObjs;
        public Location Position;
        public Tile[] Tiles;

        public override PacketType Type => PacketType.UPDATE;

        public override void Read(PacketReader r)
        {
            Position = (Location)new Location().Read(r);
            LevelType = r.ReadInt32();
            var i = 0;
            var tilesLen = CompressedInt.Read(r);
            Tiles = new Tile[tilesLen];
            for (; i < Tiles.Length; i++)
                Tiles[i] = (Tile)new Tile().Read(r);
            var newObjsLen = CompressedInt.Read(r);
            NewObjs = new Entity[newObjsLen];
            for (i = 0; i < NewObjs.Length; i++)
                NewObjs[i] = (Entity)new Entity().Read(r);
            var dropsLen = CompressedInt.Read(r);
            Drops = new int[dropsLen];
            for (i = 0; i < Drops.Length; i++)
                Drops[i] = CompressedInt.Read(r);
        }

        public override void Write(PacketWriter w)
        {
            Position.Write(w);
            w.Write(LevelType);
            CompressedInt.Write(w, Tiles.Length);
            foreach (var t in Tiles)
                t.Write(w);

            CompressedInt.Write(w, NewObjs.Length);
            foreach (var e in NewObjs)
                e.Write(w);

            CompressedInt.Write(w, Drops.Length);
            foreach (var i in Drops)
                CompressedInt.Write(w, i);
        }
    }
}