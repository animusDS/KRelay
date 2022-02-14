using Lib_K_Relay;
using Lib_K_Relay.GameData;
using Lib_K_Relay.Interface;
using Lib_K_Relay.Networking;
using Lib_K_Relay.Networking.Packets;
using Lib_K_Relay.Networking.Packets.DataObjects;
using Lib_K_Relay.Networking.Packets.Server;
using Lib_K_Relay.Utilities;
using System.Collections.Generic;

namespace IncFinder
{
    public class IncFinder : IPlugin
    {
        private readonly ushort _incId = GameData.Items.ByName("Wine Cellar Incantation").ID;
        private readonly Dictionary<int, string> _incHolders = new Dictionary<int, string>();

        public string GetAuthor()
        { return "KrazyShank / Kronks"; }

        public string GetName()
        { return "Inc Finder"; }

        public string GetDescription()
        { return "Notifies you when people have Wine Cellar Incantations in their inventories."; }

        public string[] GetCommands()
        { return new[] { "/wc" }; }

        public void Initialize(Proxy proxy)
        {
            proxy.ClientConnected += (client) => _incHolders.Clear();
            proxy.HookPacket(PacketType.UPDATE, OnUpdate);
            proxy.HookCommand("wc", OnWCCommand);
        }

        private void OnWCCommand(Client client, string command, string[] args)
        {
            string message = "Inc Holders: ";
            foreach (var pair in _incHolders)
                message += pair.Value + ",";

            client.SendToClient(PluginUtils.CreateOryxNotification("Inc Finder", message));
        }

        private void OnUpdate(Client client, Packet packet)
        {
            UpdatePacket update = (UpdatePacket)packet;

            // New Objects
            foreach (Entity entity in update.NewObjs)
            {
                bool inc = false;

                foreach (StatData statData in entity.Status.Data)
                {
                    if (!statData.IsStringData() && (statData.Id >= 8 && statData.Id <= 19) || (statData.Id >= 71 && statData.Id <= 78))
                        if (statData.IntValue == _incId) inc = true;

                    if (inc && statData.Id == (int)Stats.Name && statData.StringValue != client.PlayerData.Name)
                    {
                        if (!_incHolders.ContainsKey(entity.Status.ObjectId))
					        _incHolders.Add(entity.Status.ObjectId, statData.StringValue);

                        client.SendToClient(PluginUtils.CreateOryxNotification(
                            "Inc Finder", statData.StringValue + " has an Incantation!"));
                    }
                }
            }

            // Removed Objects
            foreach (int drop in update.Drops)
            {
                if (_incHolders.ContainsKey(drop))
                {
                    client.SendToClient(PluginUtils.CreateOryxNotification(
                            "Inc Finder", _incHolders[drop] + " has left!"));
                    _incHolders.Remove(drop);
                }
            }
        }
    }
}
