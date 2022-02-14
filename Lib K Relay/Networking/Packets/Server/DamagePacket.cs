using Lib_K_Relay.Utilities;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class DamagePacket : Packet
    {
        public bool ArmorPierce;
        public ushort BulletId;
        public ushort Damage;
        public ConditionEffectIndex[] Effects;
        public int ObjectId;
        public int TargetId;

        public override PacketType Type => PacketType.DAMAGE;

        public override void Read(PacketReader r)
        {
            TargetId = r.ReadInt32();
            Effects = new ConditionEffectIndex[r.ReadByte()];
            for (var i = 0; i < Effects.Length; i++)
                Effects[i] = (ConditionEffectIndex)r.ReadByte();

            Damage = r.ReadUInt16();
            ArmorPierce = r.ReadBoolean();
            BulletId = r.ReadUInt16();
            ObjectId = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(TargetId);
            w.Write((byte)Effects.Length);
            foreach (var c in Effects)
                w.Write((byte)c);

            w.Write(Damage);
            w.Write(ArmorPierce);
            w.Write(BulletId);
            w.Write(ObjectId);
        }
    }
}