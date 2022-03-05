using System.Collections.Generic;
using Lib_K_Relay;
using Lib_K_Relay.Interface;
using Lib_K_Relay.Networking;
using Lib_K_Relay.Networking.Packets;
using Lib_K_Relay.Networking.Packets.Server;
using Lib_K_Relay.Utilities;

namespace ReconnectMod
{
    public class ReconnectMod : IPlugin
    {
        private readonly Dictionary<string, int> _worlds = new Dictionary<string, int>
        {
            { "tutorial", -1 },
            { "nexus", -2 },
            { "randomrealm", -3 },
            { "vault", -5 },
            { "maptest", -6 },
            { "vaultexplanation", -8 },
            { "nexusexplanation", -9 },
            { "questroom", -11 },
            { "cheatersquarantine", -13 }
        };

        public string GetAuthor()
        {
            return "Animus";
        }

        public string GetName()
        {
            return "Reconnect Modifier";
        }

        public string GetDescription()
        {
            return "Reconnect to gameid cuz idk why";
        }

        public string[] GetCommands()
        {
            return new[]
            {
                "/recon 'worldname' "
            };
        }

        public void Initialize(Proxy proxy)
        {
            proxy.HookCommand("recon", (client, cmd, args) =>
            {
                if (args.Length != 1)
                {
                    client.SendToClient(PluginUtils.CreateOryxNotification("Reconnect Mod",
                        "Usage: /recon 'worldname'"));
                    return;
                }

                if (!_worlds.ContainsKey(args[0]))
                {
                    client.SendToClient(PluginUtils.CreateOryxNotification("Reconnect Mod",
                        "Not a valid reconnect destination"));
                    return;
                }

                var reconnect = (ReconnectPacket)Packet.Create(PacketType.RECONNECT);
                reconnect.Host = client.State.ConTargetAddress;
                reconnect.Port = 2050;
                reconnect.GameId = _worlds[args[0]];
                reconnect.Name = "";
                reconnect.Key = new byte[0];
                reconnect.KeyTime = -1;
                ReconnectHandler.SendReconnect(client, reconnect);
                client.SendToClient(PluginUtils.CreateOryxNotification("Reconnect Mod",
                    "Reconnecting to " + args[0] + " now..."));
            });
        }
    }
}