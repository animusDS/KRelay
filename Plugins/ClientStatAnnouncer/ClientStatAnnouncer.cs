using System;
using Lib_K_Relay;
using Lib_K_Relay.Interface;
using Lib_K_Relay.Networking;
using Lib_K_Relay.Networking.Packets;
using Lib_K_Relay.Networking.Packets.Server;
using Lib_K_Relay.Utilities;

namespace ClientStatAnnouncer
{
    public class ClientStatAnnouncer : IPlugin
    {
        public string GetAuthor()
        {
            return "KrazyShank / Kronks & Alde & Animus";
        }

        public string GetName()
        {
            return "ClientStat Announcer";
        }

        public string GetDescription()
        {
            return "Lets you know when you progress on in-game achievements.";
        }

        public string[] GetCommands()
        {
            return new[] { "/cs" };
        }

        public void Initialize(Proxy proxy)
        {
            proxy.HookPacket(PacketType.CLIENTSTAT, OnClientStat);
            proxy.HookCommand("cs", (client, cmd, args) =>
            {
                Config.Default.enabled = !Config.Default.enabled;
                Config.Default.Save();
                client.SendToClient(PluginUtils.CreateOryxNotification("Client Stat Announcer",
                    "Stats are now " + (Config.Default.enabled ? "enabled" : "disabled")));
            });
        }

        private void OnClientStat(Client client, Packet packet)
        {
            if (Config.Default.enabled == false) return;
            var clientStat = (ClientStatPacket)packet;

            string toDisplay;

            switch (clientStat.Name)
            {
                case "Shots":
                    toDisplay = "Bullets shot : " + clientStat.Value;
                    break;
                case "ShotsThatDamage":
                    toDisplay = "Bullets that damaged : " + clientStat.Value;
                    break;
                case "SpecialAbilityUses":
                    toDisplay = "Ability uses : " + clientStat.Value;
                    break;
                case "TilesUncovered":
                    toDisplay = "Tiles uncovered : " + clientStat.Value;
                    break;
                case "Teleports":
                    toDisplay = "Teleports : " + clientStat.Value;
                    break;
                case "PotionsDrunk":
                    toDisplay = "Potions drank : " + clientStat.Value;
                    break;
                case "MonsterKills":
                    toDisplay = "Monster kills : " + clientStat.Value;
                    break;
                case "MonsterAssists":
                    toDisplay = "Monster assists : " + clientStat.Value;
                    break;
                case "GodKills":
                    toDisplay = "God kills : " + clientStat.Value;
                    break;
                case "GodAssists":
                    toDisplay = "God assists : " + clientStat.Value;
                    break;
                case "CubeKills":
                    toDisplay = "Cube kills : " + clientStat.Value;
                    break;
                case "OryxKills":
                    toDisplay = "Oryx kills : " + clientStat.Value;
                    break;
                case "QuestsCompleted":
                    toDisplay = "Quests completed : " + clientStat.Value;
                    break;
                case "PirateCavesCompleted":
                    toDisplay = "Pirate Cave(s) completed : " + clientStat.Value;
                    break;
                case "UndeadLairsCompleted":
                    toDisplay = "Undead Lair(s) completed : " + clientStat.Value;
                    break;
                case "AbyssOfDemonsCompleted":
                    toDisplay = "Abyss of Demon(s) completed : " + clientStat.Value;
                    break;
                case "SnakePitsCompleted":
                    toDisplay = "Snake Pit(s) completed : " + clientStat.Value;
                    break;
                case "SpiderDensCompleted":
                    toDisplay = "Spider Den(s) completed : " + clientStat.Value;
                    break;
                case "SpriteWorldsCompleted":
                    toDisplay = "Sprite World(s) completed : " + clientStat.Value;
                    break;
                case "LevelUpAssists":
                    toDisplay = "Level-up assist(s) : " + clientStat.Value;
                    break;
                case "MinutesActive":
                    toDisplay = "Minute(s) active : " + clientStat.Value;
                    break;
                case "TombsCompleted":
                    toDisplay = "Tomb(s) completed : " + clientStat.Value;
                    break;
                case "TrenchesCompleted":
                    toDisplay = "Trenche(s) completed : " + clientStat.Value;
                    break;
                case "JunglesCompleted":
                    toDisplay = "Jungle(s) completed : " + clientStat.Value;
                    break;
                case "ManorsCompleted":
                    toDisplay = "Manor(s) completed : " + clientStat.Value;
                    break;
                case "HumanoidKills":
                    toDisplay = "Humanoid kill(s) : " + clientStat.Value;
                    break;
                case "CritterKills":
                    toDisplay = "Critter kill(s) : " + clientStat.Value;
                    break;
                default:
                    Console.WriteLine(toDisplay =
                        "Unknown -> Name :" + clientStat.Name + " Value :" + clientStat.Value);
                    break;
            }

            client.SendToClient(
                PluginUtils.CreateOryxNotification(
                    "ClientStat Announcer", toDisplay));
        }
    }
}