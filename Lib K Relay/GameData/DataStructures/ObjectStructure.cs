using System.Collections.Generic;
using System.Xml.Linq;

namespace Lib_K_Relay.GameData.DataStructures
{
    public class ObjectStructure : IDataStructure<ushort>
    {
        /// <summary>
        ///     Whether this object blocks vision
        /// </summary>
        public bool BlocksSight;

        /// <summary>
        ///     How much defense the enemy has
        /// </summary>
        public ushort Defense;

        /// <summary>
        ///     ???
        /// </summary>
        public bool DrawOnGround;

        /// <summary>
        ///     Whether this object is an enemy (e.g. can be damaged)
        /// </summary>
        public bool Enemy;

        /// <summary>
        ///     ???
        /// </summary>
        public bool EnemyOccupySquare;

        /// <summary>
        ///     Whether the enemy flies
        /// </summary>
        public bool Flying;

        /// <summary>
        ///     ???
        /// </summary>
        public bool FullOccupy;

        /// <summary>
        ///     Whether the enemy is a god (e.g. contributes to god kills)
        /// </summary>
        public bool God;

        /// <summary>
        ///     Maximum HP this object can have (for walls/other destructible terrain)
        /// </summary>
        public ushort MaxHp;

        /// <summary>
        ///     What kind of object this is
        /// </summary>
        public string ObjectClass;

        /// <summary>
        ///     Whether this object impedes movement (?)
        /// </summary>
        public bool OccupySquare;

        /// <summary>
        ///     Whether this object is a player
        /// </summary>
        public bool Player;

        /// <summary>
        ///     What projectiles this enemy can fire
        /// </summary>
        public ProjectileStructure[] Projectiles;

        /// <summary>
        ///     The size of the shadow of the enemy
        /// </summary>
        public ushort ShadowSize;

        /// <summary>
        ///     The size of the enemy
        /// </summary>
        public ushort Size;

        /// <summary>
        ///     Unknown
        /// </summary>
        public bool Static;

        /// <summary>
        ///     How much XP is granted when destroying this object
        /// </summary>
        public float XpMult;

        public ObjectStructure(XElement obj)
        {
            Id = (ushort)obj.AttrDefault("type", "0x0").ParseHex();

            // if this errors you need to add a new entry to the krObject.Class enum
            ObjectClass = obj.ElemDefault("Class", "GameObject");

            MaxHp = (ushort)obj.ElemDefault("MaxHitPoints", "0").ParseHex();
            XpMult = obj.ElemDefault("XpMult", "0").ParseFloat();

            Static = obj.HasElement("Static");
            OccupySquare = obj.HasElement("OccupySquare");
            EnemyOccupySquare = obj.HasElement("EnemyOccupySquare");
            FullOccupy = obj.HasElement("FullOccupy");
            BlocksSight = obj.HasElement("BlocksSight");
            Enemy = obj.HasElement("Enemy");
            Player = obj.HasElement("Player");
            DrawOnGround = obj.HasElement("DrawOnGround");

            Size = (ushort)obj.ElemDefault("Size", "0").ParseInt();
            ShadowSize = (ushort)obj.ElemDefault("ShadowSize", "0").ParseInt();
            Defense = (ushort)obj.ElemDefault("Defense", "0").ParseInt();
            Flying = obj.HasElement("Flying");
            God = obj.HasElement("God");

            var projs = new List<ProjectileStructure>();
            obj.Elements("Projectile").ForEach(projectile => projs.Add(new ProjectileStructure(projectile)));
            Projectiles = projs.ToArray();

            Name = obj.AttrDefault("id", "");
        }

        /// <summary>
        ///     The numerical identifier for this object
        /// </summary>
        public ushort Id { get; }

        /// <summary>
        ///     The text identifier for this object
        /// </summary>
        public string Name { get; }

        internal static Dictionary<ushort, ObjectStructure> Load(XDocument doc)
        {
            var map = new Dictionary<ushort, ObjectStructure>();

            doc.Element("Objects")
                .Elements("Object")
                .ForEach(obj =>
                {
                    var o = new ObjectStructure(obj);
                    map[o.Id] = o;
                });

            return map;
        }

        public override string ToString()
        {
            return $"Object: {Name} (0x{Id:X})";
        }
    }
}