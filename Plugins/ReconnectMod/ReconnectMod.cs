using System;
using System.Collections.Generic;
using Lib_K_Relay;
using Lib_K_Relay.Interface;
using Lib_K_Relay.Networking;
using Lib_K_Relay.Networking.Packets;
using Lib_K_Relay.Networking.Packets.Client;
using Lib_K_Relay.Networking.Packets.Server;
using Lib_K_Relay.Utilities;

namespace ReconnectMod
{
    public class ReconnectMod : IPlugin
    {
        private int _gameId;

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
                "/recon  ",
                "/recon 'worldname' ",
                "/recon 'worldname' now "
            };
        }

        public void Initialize(Proxy proxy)
        {
            proxy.HookPacket(PacketType.RECONNECT, OnReconnect);
            proxy.HookPacket(PacketType.HELLO, OnHello);
            proxy.HookPacket(PacketType.FAILURE, OnFailure);

            proxy.HookCommand("recon", (client, cmd, args) =>
            {
                switch (args.Length)
                {
                    case 1:
                        if (!_worlds.ContainsKey(args[0]))
                        {
                            client.SendToClient(PluginUtils.CreateOryxNotification("Reconnect Mod",
                                "Not a valid reconnect destination"));
                            return;
                        }

                        _gameId = _worlds[args[0]];
                        client.SendToClient(PluginUtils.CreateOryxNotification("Reconnect Mod",
                            "Reconnect modding is now set to " + args[0]));
                        return;
                    case 2:
                        if (args[1] != "now") return;
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
                        return;
                    default:
                        Config.Default.enabled = !Config.Default.enabled;
                        Config.Default.Save();
                        client.SendToClient(PluginUtils.CreateOryxNotification("Reconnect Mod",
                            "Reconnect modding is now " + (Config.Default.enabled ? "enabled" : "disabled")));
                        return;
                }
            });
        }

        private void OnReconnect(Client client, Packet packet)
        {
            if (!Config.Default.enabled) return;
            var p = (ReconnectPacket)packet;
            if (_worlds.ContainsValue(_gameId)) p.GameId = _gameId;
        }

        private void OnFailure(Client client, Packet packet)
        {
            if (!Config.Default.enabled) return;
            var p = (FailurePacket)packet;
            if (p.ErrorId == 5)
                Config.Default.enabled = false;
        }

        private void OnHello(Client client, Packet packet)
        {
            if (!Config.Default.enabled) return;
            var p = (HelloPacket)packet;
            if (_worlds.ContainsValue(_gameId)) p.GameId = _gameId;
        }
    }
}