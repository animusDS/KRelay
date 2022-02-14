using System.Reflection;
using System.Text;
using Lib_K_Relay.Networking.Packets.Server;
using Lib_K_Relay.Utilities;

namespace Lib_K_Relay.Networking.Packets.DataObjects
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
        public string DungeonMod;
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
        public Location Pos = new Location();
        public float ProjectileLifeMultiplier;
        public float ProjectileSpeedMultiplier;
        public int[] Quickslots = { -1, -1, -1 };
        public int RealmGold;
        public int SinkLevel;
        public int Size;
        public int Speed;
        public int SpeedBonus;
        public int Stars;
        public int SupporterPoints;
        public bool TeleportAllowed;
        public int Texture;
        public int Texture1;
        public int Texture2;
        public string Unknown121;
        public string Unknown123;
        public int Vitality;
        public int VitalityBonus;
        public int Wisdom;
        public int WisdomBonus;
        public int Xp;
        public int XpBoostTime;
        public int XpGoal;

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
                case (int)Stats.MaximumHP:
                    MaxHealth = intValue;
                    break;
                case (int)Stats.HP:
                    Health = intValue;
                    break;
                case (int)Stats.MaximumMP:
                    MaxMana = intValue;
                    break;
                case (int)Stats.MP:
                    Mana = intValue;
                    break;
                case (int)Stats.NextLevelExperience:
                    XpGoal = intValue;
                    break;
                case (int)Stats.Experience:
                    Xp = intValue;
                    break;
                case (int)Stats.Level:
                    Level = intValue;
                    break;
                case (int)Stats.Inventory0:
                case (int)Stats.Inventory1:
                case (int)Stats.Inventory2:
                case (int)Stats.Inventory3:
                case (int)Stats.Inventory4:
                case (int)Stats.Inventory5:
                case (int)Stats.Inventory6:
                case (int)Stats.Inventory7:
                case (int)Stats.Inventory8:
                case (int)Stats.Inventory9:
                case (int)Stats.Inventory10:
                case (int)Stats.Inventory11:
                    Inventory[id - (int)Stats.Inventory0] = intValue;
                    break;
                case (int)Stats.Backpack0:
                case (int)Stats.Backpack1:
                case (int)Stats.Backpack2:
                case (int)Stats.Backpack3:
                case (int)Stats.Backpack4:
                case (int)Stats.Backpack5:
                case (int)Stats.Backpack6:
                case (int)Stats.Backpack7:
                    Backpack[id - (int)Stats.Backpack0] = intValue;
                    break;
                case (int)Stats.Attack:
                    Attack = intValue;
                    break;
                case (int)Stats.Defense:
                    Defense = intValue;
                    break;
                case (int)Stats.Speed:
                    Speed = intValue;
                    break;
                case (int)Stats.Vitality:
                    Vitality = intValue;
                    break;
                case (int)Stats.Wisdom:
                    Wisdom = intValue;
                    break;
                case (int)Stats.Dexterity:
                    Dexterity = intValue;
                    break;
                case (int)Stats.Effects:
                    Effects[0] = intValue;
                    break;
                case (int)Stats.Effects2:
                    Effects[1] = intValue;
                    break;
                case (int)Stats.Stars:
                    Stars = intValue;
                    break;
                case (int)Stats.Name:
                    Name = stringValue;
                    break;
                case (int)Stats.Credits:
                    RealmGold = intValue;
                    break;
                case (int)Stats.AccountId:
                    AccountId = stringValue;
                    break;
                case (int)Stats.AccountFame:
                    AccountFame = intValue;
                    break;
                case (int)Stats.HealthBonus:
                    HealthBonus = intValue;
                    break;
                case (int)Stats.ManaBonus:
                    ManaBonus = intValue;
                    break;
                case (int)Stats.AttackBonus:
                    AttackBonus = intValue;
                    break;
                case (int)Stats.DefenseBonus:
                    DefenseBonus = intValue;
                    break;
                case (int)Stats.SpeedBonus:
                    SpeedBonus = intValue;
                    break;
                case (int)Stats.VitalityBonus:
                    VitalityBonus = intValue;
                    break;
                case (int)Stats.WisdomBonus:
                    WisdomBonus = intValue;
                    break;
                case (int)Stats.DexterityBonus:
                    DexterityBonus = intValue;
                    break;
                case (int)Stats.NameChosen:
                    NameChosen = intValue > 0;
                    break;
                case (int)Stats.CharacterFame:
                    CharacterFame = intValue;
                    break;
                case (int)Stats.CharacterFameGoal:
                    CharacterFameGoal = intValue;
                    break;
                case (int)Stats.LegendaryRank:
                    LegendaryRank = intValue;
                    break;
                case (int)Stats.GuildName:
                    GuildName = stringValue;
                    break;
                case (int)Stats.GuildRank:
                    GuildRank = intValue;
                    break;
                case (int)Stats.Breath:
                    Breath = intValue;
                    break;
                case (int)Stats.HasBackpack:
                    HasBackpack = intValue > 0;
                    break;
                case (int)Stats.Texture:
                    Texture = intValue;
                    break;
                case (int)Stats.SinkLevel:
                    SinkLevel = intValue;
                    break;
                case (int)Stats.Size:
                    Size = intValue;
                    break;
                case (int)Stats.QuickslotItem1:
                case (int)Stats.QuickslotItem2:
                case (int)Stats.QuickslotItem3:
                    Quickslots[id - (int)Stats.QuickslotItem1] = intValue;
                    break;
                case (int)Stats.HasQuickslotUpgrade:
                    HasQuickslotUpgrade = intValue > 0;
                    break;
                case (int)Stats.ExaltedAttack:
                    ExaltedAttack = intValue;
                    break;
                case (int)Stats.ExaltedDefense:
                    ExaltedDefense = intValue;
                    break;
                case (int)Stats.ExaltedDexterity:
                    ExaltedDexterity = intValue;
                    break;
                case (int)Stats.ExaltedHealth:
                    ExaltedHealth = intValue;
                    break;
                case (int)Stats.ExaltedMana:
                    ExaltedMana = intValue;
                    break;
                case (int)Stats.ExaltedSpeed:
                    ExaltedSpeed = intValue;
                    break;
                case (int)Stats.ExaltedVitality:
                    ExaltedVitality = intValue;
                    break;
                case (int)Stats.ExaltedWisdom:
                    ExaltedWisdom = intValue;
                    break;
                case (int)Stats.Texture1:
                    Texture1 = intValue;
                    break;
                case (int)Stats.Texture2:
                    Texture2 = intValue;
                    break;
                case (int)Stats.FortuneTokens:
                    FortuneTokens = intValue;
                    break;
                case (int)Stats.Forgefire:
                    Forgefire = intValue;
                    break;
                case (int)Stats.ExaltationDamageMultiplier:
                    ExaltationDamageMultiplier = intValue / 1000f;
                    break;
                case (int)Stats.SupporterPoints:
                    SupporterPoints = intValue;
                    break;
                case (int)Stats.ProjectileSpeedMult:
                    ProjectileSpeedMultiplier = intValue / 1000f;
                    break;
                case (int)Stats.ProjectileLifeMult:
                    ProjectileLifeMultiplier = intValue / 1000f;
                    break;
                case (int)Stats.AltTextureIndex:
                    AltTextureIndex = intValue;
                    break;
                case (int)Stats.HasXpBoost:
                    HasXpBoost = intValue > 0;
                    break;
                case (int)Stats.XpBoostTime:
                    XpBoostTime = intValue * 1000;
                    break;
                case (int)Stats.LootDropBoostTime:
                    LootDropBoostTime = intValue * 1000;
                    break;
                case (int)Stats.LootTierBoostTime:
                    LootTierBoostTime = intValue * 1000;
                    break;
                case (int)Stats.ChallengerStarBg:
                    ChallengerStarBg = intValue;
                    break;
                case (int)Stats.DungeonMod:
                    ChallengerStarBg = intValue;
                    break;
                case (int)Stats.Unknown121:
                    Unknown121 = stringValue;
                    break;
                case (int)Stats.Unknown123:
                    Unknown123 = stringValue;
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