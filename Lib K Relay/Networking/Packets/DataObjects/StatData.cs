using System;

namespace Lib_K_Relay.Networking.Packets.DataObjects
{
    public class StatData : IDataObject
    {
        public StatsType Id;
        public int IntValue;
        public int SecondaryValue;
        public string StringValue;

        public IDataObject Read(PacketReader r)
        {
            Id = r.ReadByte();
            if (IsStringData())
                StringValue = r.ReadString();
            else
                IntValue = CompressedInt.Read(r);

            SecondaryValue = CompressedInt.Read(r);

            return this;
        }

        public void Write(PacketWriter w)
        {
            w.Write(Id);
            if (IsStringData())
                w.Write(StringValue);
            else
                CompressedInt.Write(w, IntValue);

            CompressedInt.Write(w, SecondaryValue);
        }

        public object Clone()
        {
            return new StatData
            {
                Id = Id,
                IntValue = IntValue,
                StringValue = StringValue,
                SecondaryValue = SecondaryValue
            };
        }

        public bool IsStringData()
        {
            return Id.IsUtf();
        }

        public override string ToString()
        {
            return "{ Id=" + Id + " Value=" + (IsStringData() ? StringValue : IntValue.ToString()) +
                   " SecondaryValue=" + SecondaryValue + " }";
        }
    }

    public class StatsType
    {
        // this int casting nonsense is so scuffed...
        public enum Stats
        {
            MaximumHp = 0,
            Hp = 1,
            Size = 2,
            MaximumMp = 3,
            Mp = 4,
            NextLevelExperience = 5,
            Experience = 6,
            Level = 7,
            Inventory0 = 8,
            Inventory1 = 9,
            Inventory2 = 10,
            Inventory3 = 11,
            Inventory4 = 12,
            Inventory5 = 13,
            Inventory6 = 14,
            Inventory7 = 15,
            Inventory8 = 16,
            Inventory9 = 17,
            Inventory10 = 18,
            Inventory11 = 19,
            Attack = 20,
            Defense = 21,
            Speed = 22,
            Unknown23 = 23,
            Unknown24 = 24,
            Unknown25 = 25,
            Vitality = 26,
            Wisdom = 27,
            Dexterity = 28,
            Effects = 29,
            Stars = 30,
            Name = 31,
            Texture1 = 32,
            Texture2 = 33,
            MerchandiseType = 34,
            Credits = 35,
            MerchandisePrice = 36,
            PortalUsable = 37,
            AccountId = 38,
            AccountFame = 39,
            MerchandiseCurrency = 40,
            ObjectConnection = 41,
            MerchandiseRemainingCount = 42,
            MerchandiseRemainingMinutes = 43,
            MerchandiseDiscount = 44,
            MerchandiseRankRequirement = 45,
            HealthBonus = 46,
            ManaBonus = 47,
            AttackBonus = 48,
            DefenseBonus = 49,
            SpeedBonus = 50,
            VitalityBonus = 51,
            WisdomBonus = 52,
            DexterityBonus = 53,
            OwnerAccountId = 54,
            RankRequired = 55,
            NameChosen = 56,
            CharacterFame = 57,
            CharacterFameGoal = 58,
            LegendaryRank = 59,
            SinkLevel = 60,
            AltTextureIndex = 61,
            GuildName = 62,
            GuildRank = 63,
            Breath = 64,
            HasXpBoost = 65,
            XpBoostTime = 66,
            LootDropBoostTime = 67,
            LootTierBoostTime = 68,

            // unused by Exalt
            HealthPotionCount = 69,
            MagicPotionCount = 70,

            // unused by Exalt
            Backpack0 = 71,
            Backpack1 = 72,
            Backpack2 = 73,
            Backpack3 = 74,
            Backpack4 = 75,
            Backpack5 = 76,
            Backpack6 = 77,
            Backpack7 = 78,
            HasBackpack = 79,
            Texture = 80,
            PetInstanceId = 81,
            PetName = 82,
            PetType = 83,
            PetRarity = 84,
            PetMaximumLevel = 85,
            PetFamily = 86,
            PetPoints0 = 87,
            PetPoints1 = 88,
            PetPoints2 = 89,
            PetLevel0 = 90,
            PetLevel1 = 91,
            PetLevel2 = 92,
            PetAbilityType0 = 93,
            PetAbilityType1 = 94,
            PetAbilityType2 = 95,
            Effects2 = 96,
            FortuneTokens = 97,
            SupporterPoints = 98,
            IsSupporter = 99,

            // i think the star bg is unused, but still exported?
            ChallengerStarBg = 100,
            PlayerId = 101,
            OpenedAtTimestamp = 104,
            ExaltedAttack = 105,
            ExaltedDefense = 106,
            ExaltedSpeed = 107,
            ExaltedVitality = 108,
            ExaltedWisdom = 109,
            ExaltedDexterity = 110,
            ExaltedHealth = 111,
            ExaltedMana = 112,
            ExaltationDamageMultiplier = 113,
            PetOwnerAccountId = 114,
            GraveAccountId = 115,
            QuickSlotItem1 = 116,
            QuickSlotItem2 = 117,
            QuickSlotItem3 = 118,
            HasQuickSlotUpgrade = 119,
            ForgeFire = 120,
            DungeonMod = 121,
            Unknown122 = 122,
            Unknown123 = 123,
            Unknown124 = 124,
            Unknown125 = 125,
            Unknown126 = 126
        }

        private readonly byte _mType;

        private StatsType(byte type)
        {
            _mType = type;
        }

        public bool IsUtf()
        {
            return (int)this == (int)Stats.Name
                   || (int)this == (int)Stats.AccountId
                   || (int)this == (int)Stats.OwnerAccountId
                   || (int)this == (int)Stats.GuildName
                   || (int)this == (int)Stats.PetName
                   || (int)this == (int)Stats.GraveAccountId
                   || (int)this == (int)Stats.Texture
                   || (int)this == (int)Stats.DungeonMod;
        }

        public static implicit operator StatsType(int type)
        {
            if (type > byte.MaxValue) throw new Exception("Not a valid StatData number.");

            return new StatsType((byte)type);
        }

        public static implicit operator StatsType(byte type)
        {
            return new StatsType(type);
        }

        public static bool operator ==(StatsType type, int id)
        {
            if (id > byte.MaxValue) throw new Exception("Not a valid StatData number.");

            return !(type is null) && type._mType == (byte)id;
        }

        public static bool operator ==(StatsType type, byte id)
        {
            return !(type is null) && type._mType == id;
        }

        public static bool operator !=(StatsType type, int id)
        {
            if (id > byte.MaxValue) throw new Exception("Not a valid StatData number.");

            return !(type is null) && type._mType != (byte)id;
        }

        public static bool operator !=(StatsType type, byte id) => !(type is null) && type._mType != id;

        public static bool operator ==(StatsType type, StatsType id)
        {
            return !(id is null) && !(type is null) && type._mType == id._mType;
        }

        public static bool operator !=(StatsType type, StatsType id)
        {
            return !(id is null) && !(type is null) && type._mType != id._mType;
        }

        public static implicit operator int(StatsType type)
        {
            return type._mType;
        }

        public static implicit operator byte(StatsType type)
        {
            return type._mType;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is StatsType type)) return false;

            return this == type;
        }

        public override string ToString()
        {
            return _mType.ToString();
        }
    }
}