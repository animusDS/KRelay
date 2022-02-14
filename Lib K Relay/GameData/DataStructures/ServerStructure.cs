using System.Collections.Generic;
using System.Xml.Linq;

namespace Lib_K_Relay.GameData.DataStructures
{
    public struct ServerStructure : IDataStructure<string>
    {
        internal static Dictionary<string, ServerStructure> Load(XDocument doc)
        {
            var map = new Dictionary<string, ServerStructure>();

            doc.Element("Servers")
                .Elements("Server")
                .ForEach(server =>
                {
                    var s = new ServerStructure(server);
                    map[s.ID] = s;
                });

            return map;
        }

        public static readonly Dictionary<string, string> abbreviations = new Dictionary<string, string>
        {
            { "USEast2", "USE2" },
            { "EUEast", "EUE" },
            { "EUSouthWest", "EUSW" },
            { "EUNorth", "EUN" },
            { "USEast", "USE" },
            { "USWest4", "USW4" },
            { "EUWest2", "EUW2" },
            { "Asia", "ASIA" },
            { "USSouth3", "USS3" },
            { "EUWest", "EUW" },
            { "USWest", "USW" },
            { "USMidWest2", "USMW2" },
            { "USMidWest", "USMW" },
            { "USSouth", "USS" },
            { "USWest3", "USW3" },
            { "USSouthWest", "USSW" },
            { "USNorthWest", "USNW" },
            { "Australia", "AU" }
        };

        /// <summary>
        ///     The complete name of this server
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The abbreviation of this server
        /// </summary>
        public string Abbreviation;

        public string ID => Abbreviation;

        /// <summary>
        ///     The IP address of this server
        /// </summary>
        public string Address;

        public ServerStructure(XElement server)
        {
            Name = server.ElemDefault("Name", "");
            Abbreviation = abbreviations.ContainsKey(Name) ? abbreviations[Name] : "";
            Address = /*Dns.GetHostEntry(*/server.ElemDefault("DNS", "") /*).AddressList[0].ToString()*/;
        }

        public override string ToString()
        {
            return string.Format("Server: {0}/{1} ({2})", Name, Abbreviation, Address);
        }
    }
}