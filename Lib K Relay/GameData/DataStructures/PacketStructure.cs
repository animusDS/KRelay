using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Lib_K_Relay.Networking.Packets;

namespace Lib_K_Relay.GameData.DataStructures
{
    public struct PacketStructure : IDataStructure<byte>
    {
        internal static Dictionary<byte, PacketStructure> Load(XDocument doc)
        {
            var map = new Dictionary<byte, PacketStructure>();

            doc.Element("Packets")
                .Elements("Packet")
                .ForEach(packet =>
                {
                    var p = new PacketStructure(packet);
                    map[p.Id] = p;
                });

            map[255] = new PacketStructure
            {
                Id = 255,
                PacketType = PacketType.UNKNOWN,
                Type = typeof(Packet)
            };

            return map;
        }

        private static readonly Type Packet = typeof(Packet);

        private static readonly Type[] PacketTypes = Assembly.GetAssembly(typeof(Proxy)).GetTypes()
            .Where(t => Packet.IsAssignableFrom(t)).ToArray();

        /// <summary>
        ///     The numerical identifier for this packet
        /// </summary>
        public byte Id { get; private set; }

        public string Name => PacketType.ToString();

        /// <summary>
        ///     The type of this packet
        /// </summary>
        public PacketType PacketType;

        /// <summary>
        ///     The class this packet can be used as
        /// </summary>
        public Type Type;

        public PacketStructure(XElement packet)
        {
            Id = (byte)packet.AttrDefault("type", "").ParseInt();
            if (!Enum.TryParse(packet.AttrDefault("id", ""), out PacketType)) PacketType = PacketType.UNKNOWN;

            Type = null;
            foreach (var pType in PacketTypes)
            {
                var t = (Activator.CreateInstance(pType) as Packet).Type;
                if (t == PacketType) Type = pType;
            }
        }

        public override string ToString()
        {
            return string.Format("Packet: {0} (0x{1:X})", PacketType, Id);
        }
    }
}