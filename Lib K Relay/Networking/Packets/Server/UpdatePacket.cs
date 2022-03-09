using Lib_K_Relay.Networking.Packets.DataObjects.Data;
using Lib_K_Relay.Networking.Packets.DataObjects.Location;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class UpdatePacket : Packet
    {
        public int[] Drops;
        public Entity[] NewObjs;
        public Tile[] Tiles;
        public byte LevelType;
        public Location Position;
        public override PacketType Type => PacketType.UPDATE;

        public override void Read(PacketReader r)
        {
            Position = (Location) new Location().Read(r);
            LevelType = r.ReadByte();
            var tilesLen = CompressedInt.Read(r);
            Tiles = new Tile[tilesLen];
            for (var i = 0; i < Tiles.Length; i++)
                Tiles[i] = (Tile) new Tile().Read(r);

            var newObjsLen = CompressedInt.Read(r);
            NewObjs = new Entity[newObjsLen];
            for (var j = 0; j < NewObjs.Length; j++)
                NewObjs[j] = (Entity) new Entity().Read(r);

            var dropsLen = CompressedInt.Read(r);
            Drops = new int[dropsLen];
            for (var k = 0; k < Drops.Length; k++)
                Drops[k] = CompressedInt.Read(r);
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