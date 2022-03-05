using Lib_K_Relay.Networking.Packets.DataObjects.Location;
using Lib_K_Relay.Utilities;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class AoEPacket : Packet
    {
        public bool ArmorPierce;
        public int Color;
        public ushort Damage;
        public ConditionEffectIndex Effect;
        public float EffectDuration;
        public short OriginType;
        public Location Position;
        public float Radius;

        public override PacketType Type => PacketType.AOE;

        public override void Read(PacketReader r)
        {
            Position = (Location)new Location().Read(r);
            Radius = r.ReadSingle();
            Damage = r.ReadUInt16();
            Effect = (ConditionEffectIndex)r.ReadByte();
            EffectDuration = r.ReadSingle();
            OriginType = r.ReadInt16();
            Color = r.ReadInt32();
            ArmorPierce = r.ReadBoolean();
        }

        public override void Write(PacketWriter w)
        {
            Position.Write(w);
            w.Write(Radius);
            w.Write(Damage);
            w.Write((byte)Effect);
            w.Write(EffectDuration);
            w.Write(OriginType);
            w.Write(Color);
            w.Write(ArmorPierce);
        }
    }
}