using Lib_K_Relay.Networking.Packets;

namespace Lib_K_Relay.Utilities
{
    public enum ConditionEffectIndex
    {
        // First batch
        Dead = 0,
        Quiet = 1,
        Weak = 2,
        Slowed = 3,
        Sick = 4,
        Dazed = 5,
        Stunned = 6,
        Blind = 7,
        Hallucinating = 8,
        Drunk = 9,
        Confused = 10,
        StunImmume = 11,
        Invisible = 12,
        Paralyzed = 13,
        Speedy = 14,
        Bleeding = 15,
        ArmorBreakImmune = 16,
        Healing = 17,
        Damaging = 18,
        Berserk = 19,
        InCombat = 20,
        Stasis = 21,
        StasisImmune = 22,
        Invincible = 23,
        Invulnerable = 24,
        Armored = 25,
        ArmorBroken = 26,
        Hexed = 27,
        NinjaSpeedy = 28,
        Unstable = 29,
        Darkness = 30,
        // First batch

        // Second batch
        SlowedImmune = 31,
        DazedImmune = 32,
        ParalyzedImmune = 33,
        Petrified = 34,
        PetrifiedImmune = 35,
        PetStasis = 36,
        Curse = 37,
        CurseImmune = 38,
        HealthBoost = 39,
        ManaBoost = 40,
        AttackBoost = 41,
        DefenseBoost = 42,
        SpeedBoost = 43,
        VitalityBoost = 44,
        WisdomBoost = 45,
        DexterityBoost = 46,
        Silenced = 47,
        Exposed = 48,
        Energized = 49,
        HealthDebuff = 50,
        ManaDebuff = 51,
        AttackDebuff = 52,
        DefenseDebuff = 53,
        SpeedDebuff = 54,
        VitalityDebuff = 55,
        WisdomDebuff = 56,
        DexterityDebuff = 57,

        Inspired = 58
        // Second batch
    }

    public enum EffectType
    {
        Unknown = 0,
        Heal = 1,
        Teleport = 2,
        Stream = 3,
        Throw = 4,
        Nova = 5, //radius=pos1.x
        Poison = 6,
        Line = 7,
        Burst = 8, //radius=dist(pos1,pos2)
        Flow = 9,
        Ring = 10, //radius=pos1.x
        Lightning = 11, //particleSize=pos2.x
        Collapse = 12, //radius=dist(pos1,pos2)
        ConeBlast = 13, //origin=pos1, radius = pos2.x
        Earthquake = 14,
        Flash = 15, //period=pos1.x, numCycles=pos1.y
        BeachBall = 16,
        ElectricBolts = 17, //If a pet paralyzes a monster
        ElectricFlashing = 18, //If a monster got paralyzed from a electric pet
        RisingFury = 19, //If a pet is standing still (this white particles)
        NovaNoAoe = 20,
        Inspired = 21,
        HolyBeam = 22,
        CircleTelegraph = 23,
        ChaosBeam = 24,
        TeleportMonster = 25,
        Meteor = 26,
        GildedBuff = 27,
        JadeBuff = 28,
        ChaosBuff = 29,
        ThunderBuff = 30,
        StatusFlash = 31,
        FireOrbBuff = 32
    }

    public struct Argb
    {
        public byte A;
        public byte B;
        public byte G;
        public byte R;

        public Argb(uint argb)
        {
            A = (byte)((argb & 0xff000000) >> 24);
            R = (byte)((argb & 0x00ff0000) >> 16);
            G = (byte)((argb & 0x0000ff00) >> 8);
            B = (byte)((argb & 0x000000ff) >> 0);
        }

        public Argb(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public static Argb Read(PacketReader r)
        {
            var ret = new Argb();
            ret.A = r.ReadByte();
            ret.R = r.ReadByte();
            ret.G = r.ReadByte();
            ret.B = r.ReadByte();
            return ret;
        }

        public void Write(PacketWriter w)
        {
            w.Write(A);
            w.Write(R);
            w.Write(G);
            w.Write(B);
        }
    }

    public enum Bags : short
    {
        Normal = 0x500,
        Purple = 0x503,
        Pink = 0x506,
        Cyan = 0x509,
        Red = 0x510,
        Blue = 0x050B,
        Purple2 = 0x507,
        Egg = 0x508,
        White = 0x050C,
        White2 = 0x050E,
        White3 = 0x50F
    }

    public enum Ability : uint
    {
        AttackClose = 402,
        AttackMid = 404,
        AttackFar = 405,
        Electric = 406,
        Heal = 407,
        MagicHeal = 408,
        Savage = 409,
        Decoy = 410,
        RisingFury = 411
    }

    public enum Classes : short
    {
        Rogue = 0x0300,
        Archer = 0x0307,
        Wizard = 0x030e,
        Priest = 0x0310,
        Warrior = 0x031d,
        Knight = 0x031e,
        Paladin = 0x031f,
        Assassin = 0x0320,
        Necromancer = 0x0321,
        Huntress = 0x0322,
        Mystic = 0x0323,
        Trickster = 0x0324,
        Sorcerer = 0x0325,
        Ninja = 0x0326,
        Samurai = 0x0311,
        Bard = 0x031c,
        Summoner = 0x0331
    }
}