using System;
using System.Collections.Generic;
using Lib_K_Relay;
using Lib_K_Relay.Interface;
using Lib_K_Relay.Networking;
using Lib_K_Relay.Networking.Packets;
using Lib_K_Relay.Networking.Packets.Client;
using Lib_K_Relay.Networking.Packets.Server;
using Lib_K_Relay.Utilities;

namespace QoLTools
{
    public class QoLTools : IPlugin
    {
        public string GetAuthor()
        {
            return "Animus";
        }

        public string GetName()
        {
            return "QoL Tools";
        }

        public string GetDescription()
        {
            return "Helpers";
        }

        public string[] GetCommands()
        {
            return new[]
            {
                "/grank",
                "/"
            };
        }

        public void Initialize(Proxy proxy)
        {
            proxy.HookCommand("grank", (client, cmd, args) =>
            {
                if (args.Length != 2)
                {
                    client.SendToClient(PluginUtils.CreateOryxNotification("QoL Tools", "Invalid arguments. Usage: /grank <player> <rank>"));
                    return;
                }
                var packet = (ChangeGuildRankPacket)Packet.Create(PacketType.CHANGEGUILDRANK);
                packet.Name = args[0];
                packet.GuildRank = Convert.ToByte(args[1]);
                client.SendToServer(packet);
            });
        }
    }
}