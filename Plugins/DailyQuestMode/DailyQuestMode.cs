using Lib_K_Relay;
using Lib_K_Relay.Interface;
using Lib_K_Relay.Networking;
using Lib_K_Relay.Networking.Packets;
using Lib_K_Relay.Networking.Packets.Client;
using Lib_K_Relay.Utilities;

namespace DailyQuestMode
{
    public class DailyQuestMode : IPlugin
    {
        public string GetAuthor()
        {
            return "Animus";
        }

        public string GetName()
        {
            return "DailyQuestMode";
        }

        public string GetDescription()
        {
            return "Connect to daily quest room";
        }

        public string[] GetCommands()
        {
            return new[] { "dailyquestmode" };
        }

        public void Initialize(Proxy proxy)
        {
            proxy.HookCommand("dailyquestmode", (client, cmd, args) =>
            {
                Config.Default.enabled = !Config.Default.enabled;
                Config.Default.Save();
                client.SendToClient(PluginUtils.CreateOryxNotification("Daily Quest Mode",
                    "Daily Quest Mode is now " + (Config.Default.enabled ? "enabled" : "disabled")));
            });
            proxy.HookPacket(PacketType.LOAD, OnLoad);
        }

        private bool _firstLoad = true;

        private void OnLoad(Client client, Packet packet)
        {
            if (!Config.Default.enabled) return;
            if (!_firstLoad) return;
            packet.Send = false;
            _firstLoad = false;
            client.SendToServer((GoToQuestRoomPacket)Packet.Create(PacketType.QUEST_ROOM_MSG));
        }
    }
}