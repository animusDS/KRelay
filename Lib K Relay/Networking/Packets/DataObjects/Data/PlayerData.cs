using System;
using System.Reflection;
using System.Text;
using Lib_K_Relay.Networking.Packets.DataObjects.Stat;
using Lib_K_Relay.Networking.Packets.Server;
using Lib_K_Relay.Utilities;

namespace Lib_K_Relay.Networking.Packets.DataObjects.Data
{
    public class PlayerData // TODO: Add the rest of the stats
    {
        public int AccountFame;

        public string AccountId;

        // future-proofing: technically a possible Player value, but never ends up being one
        public int AltTextureIndex;
        public int Attack;
        public int AttackBonus;
        public int[] Backpack = { -1, -1, -1, -1, -1, -1, -1, -1 };
        public int Breath;
        public int ChallengerStarBg;
        public int CharacterFame;
        public int CharacterFameGoal;
        public Classes Class;
        public int Defense;
        public int DefenseBonus;
        public int Dexterity;
        public int DexterityBonus;
        public int[] Effects = new int[2];
        public float ExaltationDamageMultiplier;
        public int ExaltedAttack;
        public int ExaltedDefense;
        public int ExaltedDexterity;
        public int ExaltedHealth;
        public int ExaltedMana;
        public int ExaltedSpeed;
        public int ExaltedVitality;
        public int ExaltedWisdom;
        public int Forgefire;
        public int FortuneTokens;
        public string GuildName;
        public int GuildRank;
        public bool HasBackpack;
        public bool HasQuickslotUpgrade;
        public bool HasXpBoost;
        public int Health;
        public int HealthBonus;
        public string PetName;
        public int[] Inventory = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        public int LegendaryRank;
        public int Level = 1;
        public int LootDropBoostTime;
        public int LootTierBoostTime;
        public int Mana;
        public int ManaBonus;
        public int MapHeight;
        public string MapName;
        public int MapWidth;
        public int MaxHealth;
        public int MaxMana;
        public string Name;
        public bool NameChosen;
        public int OwnerObjectId;
        public string OwnerAccountId;
        public string GraveAccountId;
        public Location.Location Pos = new Location.Location();
        public int[] Quickslots = { -1, -1, -1 };
        public int RealmGold;
        public int SinkLevel;
        public int Size;
        public int Speed;
        public int SpeedBonus;
        public int Stars;
        public int SupporterPoints;
        public bool TeleportAllowed;
        public int Unknown23;
        public int Unknown24;
        public int Unknown25;
        public int Texture1;
        public string Texture;
        public int Texture2;
        public int Texture3;
        public int Vitality;
        public int VitalityBonus;
        public int Wisdom;
        public int WisdomBonus;
        public string Xp;
        public int XpBoostTime;
        public int XpGoal;
        public string DungeonMod;
        public int Unknown122;
        public int Unknown123;
        public int Unknown124;
        public int Unknown125;
        public int Unknown126;

        public PlayerData(int ownerObjectId)
        {
            OwnerObjectId = ownerObjectId;
            Name = "";
        }

        public PlayerData(int ownerObjectId, MapInfoPacket mapInfo)
        {
            OwnerObjectId = ownerObjectId;
            Name = "";
            MapName = mapInfo.Name;
            TeleportAllowed = mapInfo.AllowPlayerTeleport;
            MapWidth = mapInfo.Width;
            MapHeight = mapInfo.Height;
        }

        public void Parse(UpdatePacket update)
        {
            foreach (var newObject in update.NewObjs)
                if (newObject.Status.ObjectId == OwnerObjectId)
                {
                    Class = (Classes)newObject.ObjectType;
                    foreach (var data in newObject.Status.Data) Parse(data.Id, data.IntValue, data.StringValue);
                }
        }

        public void Parse(NewTickPacket newTick)
        {
            foreach (var status in newTick.Statuses)
                if (status.ObjectId == OwnerObjectId)
                    foreach (var data in status.Data)
                    {
                        Pos = status.Position;
                        Parse(data.Id, data.IntValue, data.StringValue);
                    }
        }

        public void Parse(int id, int intValue, string stringValue)
        {
            switch (id)
            {
                case (int)StatsType.Stats.Hp: // 0
                    Health = intValue;
                    break;
                case (int)StatsType.Stats.MaximumMp: // 3
                    MaxMana = intValue;
                    break;
                case (int)StatsType.Stats.Mp: // 4
                    Mana = intValue;
                    break;
                case (int)StatsType.Stats.NextLevelExperience: // 5
                    XpGoal = intValue;
                    break;
                case (int)StatsType.Stats.Experience: // 6
                    Xp = stringValue;
                    break;
                case (int)StatsType.Stats.Level: // 7
                    Level = intValue;
                    break;
                case (int)StatsType.Stats.Inventory0: // 8
                case (int)StatsType.Stats.Inventory1: // 9
                case (int)StatsType.Stats.Inventory2: // 10
                case (int)StatsType.Stats.Inventory3: // 11
                case (int)StatsType.Stats.Inventory4: // 12
                case (int)StatsType.Stats.Inventory5: // 13
                case (int)StatsType.Stats.Inventory6: // 14
                case (int)StatsType.Stats.Inventory7: // 15
                case (int)StatsType.Stats.Inventory8: // 16
                case (int)StatsType.Stats.Inventory9: // 17
                case (int)StatsType.Stats.Inventory10: // 18
                case (int)StatsType.Stats.Inventory11: // 19
                    Inventory[id - (int)StatsType.Stats.Inventory0] = intValue;
                    break;
                case (int)StatsType.Stats.Attack: // 
                    Attack = intValue;
                    break;
                case (int)StatsType.Stats.Defense: // 
                    Defense = intValue;
                    break;
                case (int)StatsType.Stats.Speed: // 
                    Speed = intValue;
                    break;
                case (int)StatsType.Stats.Vitality: // 
                    Vitality = intValue;
                    break;
                case (int)StatsType.Stats.Wisdom: // 
                    Wisdom = intValue;
                    break;
                case (int)StatsType.Stats.Dexterity: // 
                    Dexterity = intValue;
                    break;
                case (int)StatsType.Stats.Effects: // 
                    Effects[0] = intValue;
                    break;
                case (int)StatsType.Stats.Stars: // 
                    Stars = intValue;
                    break;
                case (int)StatsType.Stats.Name: // 
                    Name = stringValue;
                    break;
                case (int)StatsType.Stats.Credits: // 
                    RealmGold = intValue;
                    break;
                case (int)StatsType.Stats.AccountId: // 
                    AccountId = stringValue;
                    break;
                case (int)StatsType.Stats.AccountFame: // 
                    AccountFame = intValue;
                    break;
                case (int)StatsType.Stats.PetName: // 
                    PetName = stringValue;
                    break;
                case (int)StatsType.Stats.HealthBonus: // 
                    HealthBonus = intValue;
                    break;
                case (int)StatsType.Stats.ManaBonus: // 
                    ManaBonus = intValue;
                    break;
                case (int)StatsType.Stats.AttackBonus: // 
                    AttackBonus = intValue;
                    break;
                case (int)StatsType.Stats.DefenseBonus: // 
                    DefenseBonus = intValue;
                    break;
                case (int)StatsType.Stats.SpeedBonus: // 
                    SpeedBonus = intValue;
                    break;
                case (int)StatsType.Stats.VitalityBonus: // 
                    VitalityBonus = intValue;
                    break;
                case (int)StatsType.Stats.WisdomBonus: // 
                    WisdomBonus = intValue;
                    break;
                case (int)StatsType.Stats.DexterityBonus: // 
                    DexterityBonus = intValue;
                    break;
                case (int)StatsType.Stats.NameChosen: // 
                    NameChosen = intValue > 0;
                    break;
                case (int)StatsType.Stats.CharacterFame: // 
                    CharacterFame = intValue;
                    break;
                case (int)StatsType.Stats.CharacterFameGoal: // 
                    CharacterFameGoal = intValue;
                    break;
                case (int)StatsType.Stats.LegendaryRank: // 
                    LegendaryRank = intValue;
                    break;
                case (int)StatsType.Stats.GuildName: // 
                    GuildName = stringValue;
                    break;
                case (int)StatsType.Stats.GraveAccountId: // 
                    GraveAccountId = stringValue;
                    break;
                case (int)StatsType.Stats.OwnerAccountId: // 
                    OwnerAccountId = stringValue;
                    break;
                case (int)StatsType.Stats.GuildRank: // 
                    GuildRank = intValue;
                    break;
                case (int)StatsType.Stats.Breath: // 
                    Breath = intValue;
                    break;
                case (int)StatsType.Stats.Backpack0: // 
                case (int)StatsType.Stats.Backpack1: // 
                case (int)StatsType.Stats.Backpack2: // 
                case (int)StatsType.Stats.Backpack3: // 
                case (int)StatsType.Stats.Backpack4: // 
                case (int)StatsType.Stats.Backpack5: // 
                case (int)StatsType.Stats.Backpack6: // 
                case (int)StatsType.Stats.Backpack7: // 
                    Backpack[id - (int)StatsType.Stats.Backpack0] = intValue;
                    break;
                case (int)StatsType.Stats.HasBackpack: // 
                    HasBackpack = intValue > 0;
                    break;
                case (int)StatsType.Stats.Unknown23: // 
                    Unknown23 = intValue;
                    break;
                case (int)StatsType.Stats.Unknown24: // 
                    Unknown24 = intValue;
                    break;
                case (int)StatsType.Stats.Unknown25: // 
                    Unknown25 = intValue;
                    break;
                case (int)StatsType.Stats.SinkLevel: // 
                    SinkLevel = intValue;
                    break;
                case (int)StatsType.Stats.Size: // 
                    Size = intValue;
                    break;
                case (int)StatsType.Stats.QuickSlotItem1: // 
                case (int)StatsType.Stats.QuickSlotItem2: // 
                case (int)StatsType.Stats.QuickSlotItem3: // 
                    Quickslots[id - (int)StatsType.Stats.QuickSlotItem1] = intValue;
                    break;
                case (int)StatsType.Stats.HasQuickSlotUpgrade: // 
                    HasQuickslotUpgrade = intValue > 0;
                    break;
                case (int)StatsType.Stats.ExaltedAttack: // 
                    ExaltedAttack = intValue;
                    break;
                case (int)StatsType.Stats.ExaltedDefense: // 
                    ExaltedDefense = intValue;
                    break;
                case (int)StatsType.Stats.ExaltedDexterity: // 
                    ExaltedDexterity = intValue;
                    break;
                case (int)StatsType.Stats.ExaltedHealth: // 
                    ExaltedHealth = intValue;
                    break;
                case (int)StatsType.Stats.ExaltedMana: // 
                    ExaltedMana = intValue;
                    break;
                case (int)StatsType.Stats.ExaltedSpeed: // 
                    ExaltedSpeed = intValue;
                    break;
                case (int)StatsType.Stats.ExaltedVitality: // 
                    ExaltedVitality = intValue;
                    break;
                case (int)StatsType.Stats.ExaltedWisdom: // 
                    ExaltedWisdom = intValue;
                    break;
                case (int)StatsType.Stats.Effects2: // 
                    Effects[1] = intValue;
                    break;
                case (int)StatsType.Stats.FortuneTokens: // 97
                    FortuneTokens = intValue;
                    break;
                case (int)StatsType.Stats.ExaltationDamageMultiplier: // 113
                    ExaltationDamageMultiplier = intValue / 1000f;
                    break;
                case (int)StatsType.Stats.SupporterPoints: // 98
                    SupporterPoints = intValue;
                    break;
                case (int)StatsType.Stats.AltTextureIndex: // 61
                    AltTextureIndex = intValue;
                    break;
                case (int)StatsType.Stats.HasXpBoost: // 65
                    HasXpBoost = intValue > 0;
                    break;
                case (int)StatsType.Stats.XpBoostTime: // 66
                    XpBoostTime = intValue * 1000;
                    break;
                case (int)StatsType.Stats.LootDropBoostTime: // 67
                    LootDropBoostTime = intValue * 1000;
                    break;
                case (int)StatsType.Stats.LootTierBoostTime: // 68
                    LootTierBoostTime = intValue * 1000;
                    break;
                case (int)StatsType.Stats.ChallengerStarBg: // 100
                    ChallengerStarBg = intValue;
                    break;
                case (int)StatsType.Stats.ForgeFire: // 120
                    Texture = stringValue;
                    break;
                case (int)StatsType.Stats.DungeonMod: // 121
                    DungeonMod = stringValue;
                    break;
                case (int)StatsType.Stats.Unknown122: // 122
                    Unknown122 = intValue;
                    break;
                case (int)StatsType.Stats.Unknown123: // 123
                    Unknown123 = intValue;
                    break;
                case (int)StatsType.Stats.Unknown124: // 124
                    Unknown124 = intValue;
                    break;
                case (int)StatsType.Stats.Unknown125: // 125
                    Unknown125 = intValue;
                    break;
                case (int)StatsType.Stats.Unknown126: // 126
                    Unknown126 = intValue;
                    break;    
            }
        }

        public bool HasConditionEffect(ConditionEffectIndex effect)
        {
            return (int)effect > 30 ? (Effects[1] & (int)effect) != 0 : (Effects[0] & (int)effect) != 0;
        }

        public override string ToString()
        {
            // Use reflection to get the the non-null fields and arrange them into a table.
            var fields = GetType().GetFields(BindingFlags.Public |
                                             BindingFlags.NonPublic |
                                             BindingFlags.Instance);

            var s = new StringBuilder();
            s.Append(OwnerObjectId + "'s PlayerData Instance");
            foreach (var f in fields)
                if (f.GetValue(this) != null)
                    s.Append("\n\t" + f.Name + " => " + f.GetValue(this));

            return s.ToString();
        }
    }
}