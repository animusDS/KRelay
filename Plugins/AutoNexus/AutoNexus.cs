using System;
using System.Collections.Generic;
using System.Linq;
using Lib_K_Relay;
using Lib_K_Relay.GameData;
using Lib_K_Relay.Interface;
using Lib_K_Relay.Networking;
using Lib_K_Relay.Networking.Packets;
using Lib_K_Relay.Networking.Packets.Client;
using Lib_K_Relay.Networking.Packets.DataObjects.Stat;
using Lib_K_Relay.Networking.Packets.Server;
using Lib_K_Relay.Utilities;
using MapCacher;

namespace AutoNexus
{
    internal static class Extensions
    {
        public static void OryxMessage(this Client client, string fmt, params object[] args)
        {
            client.SendToClient(PluginUtils.CreateOryxNotification("Auto Nexus", string.Format(fmt, args)));
        }
    }

    internal struct Bullet
    {
        /// <summary>
        ///     Map of piercing projectiles
        ///     Object ID -> list of piercing projectile IDs
        /// </summary>
        public static Dictionary<int, List<int>> piercing = new Dictionary<int, List<int>>();

        /// <summary>
        ///     Map of armor break projectiles
        ///     Object ID -> list of armor break projectile IDs
        /// </summary>
        public static Dictionary<int, List<int>> breaking = new Dictionary<int, List<int>>();

        public static bool IsPiercing(int enemyType, int projectileType)
        {
            return piercing.ContainsKey(enemyType) && piercing[enemyType].Contains(projectileType);
        }

        public static bool IsArmorBreaking(int enemyType, int projectileType)
        {
            return breaking.ContainsKey(enemyType) && breaking[enemyType].Contains(projectileType);
        }

        // owner ID of bullet
        public int OwnerID;

        // the ID of the bullet
        public int ID;

        // the type of projectile
        public int ProjectileID;

        // raw damage
        public int Damage;
    }

    internal class ClientState
    {
        public bool ArmorBroken;

        public bool Armored;

        // enemy id -> projectiles
        public Dictionary<int, List<Bullet>> BulletMap = new Dictionary<int, List<Bullet>>();

        public Client client;

        // enemy id -> enemy type
        public Dictionary<int, ushort> EnemyTypeMap = new Dictionary<int, ushort>();
        public int HP = 100;

        public bool safe = true;

        public ClientState(Client client)
        {
            this.client = client;
        }

        public void Update(UpdatePacket update)
        {
            foreach (var e in update.NewObjs)
                if (!EnemyTypeMap.ContainsKey(e.Status.ObjectId))
                    EnemyTypeMap[e.Status.ObjectId] = e.ObjectType;
        }

        public void Tick(NewTickPacket tick)
        {
            var regenHP = (int)(0.2 + client.PlayerData.Vitality * 0.024);
            if (!client.PlayerData.HasConditionEffect(ConditionEffectIndex.InCombat))
                regenHP *= 2;
            HP += regenHP;

            foreach (var status in tick.Statuses)
                if (status.ObjectId == client.ObjectId)
                    foreach (var stat in status.Data)
                        if (stat.Id == (int)StatsType.Stats.Hp)
                            HP = stat.IntValue;

            ArmorBroken = client.PlayerData.HasConditionEffect(ConditionEffectIndex.ArmorBroken);
            Armored = client.PlayerData.HasConditionEffect(ConditionEffectIndex.Armored);
        }

        private void MapBullet(Bullet b)
        {
            if (!BulletMap.ContainsKey(b.OwnerID))
                BulletMap[b.OwnerID] = new List<Bullet>();

            BulletMap[b.OwnerID].Add(b);
        }

        public void EnemyShoot(EnemyShootPacket eshoot)
        {
            for (var i = 0; i < eshoot.NumShots; i++)
            {
                var b = new Bullet
                {
                    OwnerID = eshoot.OwnerId,
                    ID = eshoot.BulletId + i,
                    ProjectileID = eshoot.BulletType,
                    Damage = eshoot.Damage
                };
                MapBullet(b);
            }
        }

        private int PredictDamage(AoEPacket aoe)
        {
            var def = client.PlayerData.Defense;

            if (aoe.Effect == ConditionEffectIndex.ArmorBroken)
                ArmorBroken = true;

            if (Armored) def *= 2;

            if (ArmorBroken) def = 0;

            return Math.Max(Math.Max(aoe.Damage - def, 0), (int)(0.15f * aoe.Damage));
        }

        private int PredictDamage(Bullet b)
        {
            var def = client.PlayerData.Defense;

            if (EnemyTypeMap.ContainsKey(b.OwnerID) &&
                Bullet.IsArmorBreaking(EnemyTypeMap[b.OwnerID], b.ProjectileID) && !ArmorBroken)
            {
                ArmorBroken = true;

                if (Config.Default.Debug)
                    PluginUtils.Log("Auto Nexus", "{0}'s armor is broken!", client.PlayerData.Name);
            }

            if (Armored) def *= 2;

            if (ArmorBroken || EnemyTypeMap.ContainsKey(b.OwnerID) &&
                Bullet.IsPiercing(EnemyTypeMap[b.OwnerID], b.ProjectileID))
                def = 0;

            return Math.Max(Math.Max(b.Damage - def, 0), (int)(0.15f * b.Damage));
        }

        private bool ApplyDamage(int dmg)
        {
            if (!safe) return false;
            HP -= dmg;

            if (Config.Default.Debug)
                PluginUtils.Log("Auto Nexus", "{0} was hit for {1} damage ({2}/{3})!", client.PlayerData.Name, dmg, HP,
                    client.PlayerData.MaxHealth);

            if (Config.Default.Enabled && (float)HP / client.PlayerData.MaxHealth <= Config.Default.NexusPercent)
            {
                PluginUtils.Log("Auto Nexus", "Saved {0} at {1}/{2} HP!", client.PlayerData.Name, HP,
                    client.PlayerData.MaxHealth);
                client.SendToServer(Packet.Create(PacketType.ESCAPE));
                safe = false;
                return false;
            }

            return true;
        }

        public void PlayerHit(PlayerHitPacket phit)
        {
            foreach (var b in BulletMap[phit.ObjectId])
                if (b.ID == phit.BulletId)
                {
                    phit.Send = ApplyDamage(PredictDamage(b));
                    break;
                }
        }

        public void AoE(AoEPacket aoe)
        {
            if (client.PlayerData.Pos.DistanceSquaredTo(aoe.Position) <= aoe.Radius * aoe.Radius)
                aoe.Send = ApplyDamage(PredictDamage(aoe));
        }

        public void Notification(NotificationPacket notif)
        {
            // parse +hp notifs
        }

        public void GroundDamage(GroundDamagePacket gdamage)
        {
            var t = client.GetMap().At(gdamage.Position.X, gdamage.Position.Y);
            if (GameData.Tiles.Map.ContainsKey(t)) gdamage.Send = ApplyDamage(GameData.Tiles.ById(t).MaxDamage);
        }
    }

    public class AutoNexus : IPlugin
    {
        private Dictionary<Client, ClientState> clients;

        public string GetAuthor()
        {
            return "apemanzilla";
        }

        public string[] GetCommands()
        {
            return new[]
            {
                "/autonexus",
                "/autonexus [percentage] - set the percentage to go nexus at (0-99)",
                "/autonexus [on | off] - toggle autonexus on and off",
                "/autonexus debug [on | off] - toggle debug messages on and off"
            };
        }

        public string GetDescription()
        {
            return "Attempts to save you from death by nexusing before a fatal blow." +
                   "\nThis plugin will NOT make you completely invulnerable, but it will definitely help prevent you from dying!";
        }

        public string GetName()
        {
            return "Auto Nexus";
        }

        public void Initialize(Proxy proxy)
        {
            GameData.Objects.Map
                .ForEach(enemy =>
                {
                    // armor piercing
                    if (enemy.Value.Projectiles.Any(p => p.ArmorPiercing))
                    {
                        Bullet.piercing[enemy.Value.Id] = new List<int>();
                        enemy.Value.Projectiles.ForEach(proj =>
                        {
                            if (proj.ArmorPiercing)
                                Bullet.piercing[enemy.Value.Id].Add(proj.Id);
                        });
                    }

                    // armor break
                    if (enemy.Value.Projectiles.Any(p => p.StatusEffects.ContainsKey("Armor Broken")))
                    {
                        Bullet.breaking[enemy.Value.Id] = new List<int>();
                        enemy.Value.Projectiles.ForEach(proj =>
                        {
                            if (proj.StatusEffects.ContainsKey("Armor Broken"))
                                Bullet.breaking[enemy.Value.Id].Add(proj.Id);
                        });
                    }
                });

            clients = new Dictionary<Client, ClientState>();

            proxy.HookCommand("autonexus", OnCommand);

            proxy.ClientConnected += OnConnect;
            proxy.ClientDisconnected += OnDisconnect;

            proxy.HookPacket(PacketType.UPDATE, OnPacket);
            proxy.HookPacket(PacketType.NEWTICK, OnPacket);
            proxy.HookPacket(PacketType.ENEMYSHOOT, OnPacket);
            proxy.HookPacket(PacketType.PLAYERHIT, OnPacket);
            proxy.HookPacket(PacketType.AOE, OnPacket);
            proxy.HookPacket(PacketType.GROUNDDAMAGE, OnPacket);
            proxy.HookPacket(PacketType.NOTIFICATION, OnPacket);

            // force map cacher to load
            MapCacher.MapCacher.ForceLoad();
        }

        private void OnCommand(Client client, string command, string[] args)
        {
            if (args.Length == 0)
            {
                client.OryxMessage("Auto Nexus is {0}.", Config.Default.Enabled ? "enabled" : "disabled");
                if (Config.Default.Enabled)
                    client.OryxMessage("You will be sent to the nexus at <={0}% HP.",
                        (int)(Config.Default.NexusPercent * 100));
            }
            else
            {
                switch (args[0])
                {
                    case "on":
                        Config.Default.Enabled = true;
                        Config.Default.Save();
                        client.OryxMessage("Auto Nexus now enabled.");
                        break;

                    case "off":
                        Config.Default.Enabled = false;
                        Config.Default.Save();
                        client.OryxMessage("Auto Nexus now disabled.");
                        break;

                    case "debug":
                        if (args[1] == "on")
                        {
                            Config.Default.Debug = true;
                            Config.Default.Save();
                            client.OryxMessage("Debug output enabled.");
                        }
                        else if (args[1] == "off")
                        {
                            Config.Default.Debug = false;
                            Config.Default.Save();
                            client.OryxMessage("Debug output disabled.");
                        }
                        else
                        {
                            client.OryxMessage("Unrecognized argument: {0}", args[0]);
                            client.OryxMessage("Usage:");
                            client.OryxMessage("'/autonexus on' - enable autonexus");
                            client.OryxMessage("'/autonexus off' - disable autonexus");
                            client.OryxMessage("'/autonexus 10' - set autonexus percentage to 10%");
                        }

                        break;

                    default:
                        int percentage;
                        if (int.TryParse(args[0], out percentage))
                        {
                            if (percentage > 99 || percentage < 0)
                            {
                                client.OryxMessage("Percentage should be between 0 and 99.");
                            }
                            else
                            {
                                Config.Default.NexusPercent = (float)percentage / 100;
                                Config.Default.Save();
                                client.OryxMessage("Auto Nexus percentage set to {0}%.", percentage);
                            }
                        }
                        else
                        {
                            client.OryxMessage("Unrecognized argument: {0}", args[0]);
                            client.OryxMessage("Usage:");
                            client.OryxMessage("'/autonexus on' - enable autonexus");
                            client.OryxMessage("'/autonexus off' - disable autonexus");
                            client.OryxMessage("'/autonexus 10' - set autonexus percentage to 10%");
                        }

                        break;
                }
            }
        }

        private void OnConnect(Client client)
        {
            clients[client] = new ClientState(client);
        }

        private void OnDisconnect(Client client)
        {
            if (clients.ContainsKey(client)) clients.Remove(client);
        }

        private void OnPacket(Client client, Packet p)
        {
            if (clients.ContainsKey(client))
            {
                var state = clients[client];
                switch (p.Type)
                {
                    case PacketType.UPDATE:
                        state.Update(p as UpdatePacket);
                        break;

                    case PacketType.NEWTICK:
                        state.Tick(p as NewTickPacket);
                        break;

                    case PacketType.ENEMYSHOOT:
                        state.EnemyShoot(p as EnemyShootPacket);
                        break;

                    case PacketType.PLAYERHIT:
                        state.PlayerHit(p as PlayerHitPacket);
                        break;

                    case PacketType.AOE:
                        state.AoE(p as AoEPacket);
                        break;

                    case PacketType.GROUNDDAMAGE:
                        state.GroundDamage(p as GroundDamagePacket);
                        break;

                    case PacketType.NOTIFICATION:
                        state.Notification(p as NotificationPacket);
                        break;
                }
            }
        }
    }
}