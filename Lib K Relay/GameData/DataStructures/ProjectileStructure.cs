using System.Collections.Generic;
using System.Xml.Linq;

namespace Lib_K_Relay.GameData.DataStructures
{
    public class ProjectileStructure : IDataStructure<byte>
    {
        public int Acceleration;

        public float AccelerationDelay;
        public float Amplitude;
        public bool ArmorPiercing;
        public bool Boomerang;

        /// <summary>
        ///     How much damage the projectile deals
        /// </summary>
        public int Damage;

        public float Frequency;

        /// <summary>
        ///     The lifetime of the projectile, in milliseconds
        /// </summary>
        public float Lifetime;

        public float Magnitude;

        public int MaxDamage;
        public int MinDamage;
        public bool MultiHit;
        public bool Parametric;
        public bool PassesCover;

        /// <summary>
        ///     The size of the projectile
        /// </summary>
        public int Size;

        /// <summary>
        ///     How fast the projectile moves
        /// </summary>
        public float Speed;

        public int SpeedClamp;

        /// <summary>
        ///     What status effects, if any, the projectile applies (name: duration in seconds)
        /// </summary>
        public Dictionary<string, float> StatusEffects;

        public bool Wavy;

        public ProjectileStructure(XElement projectile)
        {
            ID = (byte)projectile.AttrDefault("id", "0").ParseInt();
            Damage = projectile.ElemDefault("Damage", "0").ParseInt();
            Speed = projectile.ElemDefault("Speed", "0").ParseFloat() / 10000f;
            Size = projectile.ElemDefault("Size", "0").ParseInt();
            Lifetime = projectile.ElemDefault("LifetimeMS", "0").ParseFloat();

            MaxDamage = projectile.ElemDefault("MaxDamage", "0").ParseInt();
            MinDamage = projectile.ElemDefault("MinDamage", "0").ParseInt();
            Acceleration = projectile.ElemDefault("Acceleration", "0").ParseInt();
            AccelerationDelay = projectile.ElemDefault("AccelerationDelay", "0").ParseFloat();
            SpeedClamp = projectile.ElemDefault("SpeedClamp", "-1").ParseInt();

            Magnitude = projectile.ElemDefault("Magnitude", "0").ParseFloat();
            Amplitude = projectile.ElemDefault("Amplitude", "0").ParseFloat();
            Frequency = projectile.ElemDefault("Frequency", "0").ParseFloat();

            Wavy = projectile.HasElement("Wavy");
            Parametric = projectile.HasElement("Parametric");
            Boomerang = projectile.HasElement("Boomerang");
            ArmorPiercing = projectile.HasElement("ArmorPiercing");
            MultiHit = projectile.HasElement("MultiHit");
            PassesCover = projectile.HasElement("PassesCover");

            var effects = new Dictionary<string, float>();
            projectile.Elements("ConditionEffect")
                .ForEach(effect => effects[effect.Value] = effect.AttrDefault("duration", "0").ParseFloat());

            StatusEffects = effects;
            Name = projectile.ElemDefault("ObjectId", "");
        }

        /// <summary>
        ///     The numerical identifier for this projectile
        /// </summary>
        public byte ID { get; }

        /// <summary>
        ///     The text identifier for this projectile
        /// </summary>
        public string Name { get; }

        public override string ToString()
        {
            return $"Projectile: {Name} (0x{ID:X})";
        }
    }
}