using System.Collections.Generic;
using System.Xml.Linq;

namespace Lib_K_Relay.GameData.DataStructures
{
    public struct TileStructure : IDataStructure<ushort>
    {
        internal static Dictionary<ushort, TileStructure> Load(XDocument doc)
        {
            var map = new Dictionary<ushort, TileStructure>();

            doc.Element("GroundTypes")
                .Elements("Ground")
                .ForEach(tile =>
                {
                    var t = new TileStructure(tile);
                    map[t.Id] = t;
                });

            return map;
        }

        /// <summary>
        ///     The numerical identifier of this tile
        /// </summary>
        public ushort Id { get; }

        /// <summary>
        ///     Whether this tile can be walked on
        /// </summary>
        public bool NoWalk;

        /// <summary>
        ///     How fast you walk across this tile
        /// </summary>
        public float Speed;

        /// <summary>
        ///     Whether you sink into this tile visually (e.g. water)
        /// </summary>
        public bool Sink;

        /// <summary>
        ///     The minimum damage this tile can deal (e.g. lava)
        /// </summary>
        public ushort MinDamage;

        /// <summary>
        ///     The maximum damage this tile can deal (e.g. lava)
        /// </summary>
        public ushort MaxDamage;

        /// <summary>
        ///     The text identifier for this tile
        /// </summary>
        public string Name { get; }

        public TileStructure(XElement tile)
        {
            Id = (ushort)tile.AttrDefault("type", "0x0").ParseHex();
            NoWalk = tile.HasElement("NoWalk");
            Speed = tile.ElemDefault("Speed", "1").ParseFloat();
            Sink = tile.HasElement("Sink");
            MinDamage = (ushort)tile.ElemDefault("MinDamage", "0").ParseInt();
            MaxDamage = (ushort)tile.ElemDefault("MaxDamage", "0").ParseInt();

            Name = tile.AttrDefault("id", "");
        }

        public override string ToString()
        {
            return $"Tile: {Name} (0x{Id:X})";
        }
    }
}