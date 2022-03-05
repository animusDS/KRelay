using Lib_K_Relay.Networking.Packets.DataObjects.Data;
using Lib_K_Relay.Networking.Packets.DataObjects.Location;
using Lib_K_Relay.Utilities;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class ShowEffectPacket : Packet
    {
        public Argb Color;
        public double Duration;
        public EffectType EffectType;
        public byte ExportBitMask;
        public Location PosA;
        public Location PosB;
        public int Size;
        public int TargetId;

        public override PacketType Type => PacketType.SHOWEFFECT;

        public override void Read(PacketReader r)
        {
            EffectType = (EffectType)r.ReadByte();
            ExportBitMask = r.ReadByte();
            TargetId = (ExportBitMask & 64) != 0 ? CompressedInt.Read(r) : 0;
            PosA = new Location
            {
                X = (ExportBitMask & 2) != 0 ? r.ReadSingle() : 0,
                Y = (ExportBitMask & 4) != 0 ? r.ReadSingle() : 0
            };
            PosB = new Location
            {
                X = (ExportBitMask & 8) != 0 ? r.ReadSingle() : 0,
                Y = (ExportBitMask & 16) != 0 ? r.ReadSingle() : 0
            };
            Color = (ExportBitMask & 1) != 0 ? Argb.Read(r) : new Argb(0xFFFFFFFF);
            Duration = (ExportBitMask & 32) != 0 ? r.ReadSingle() : 1;
            Size = (ExportBitMask & 128) != 0 ? r.ReadByte() : 100;
        }

        public override void Write(PacketWriter w)
        {
            w.Write((byte)EffectType);
            w.Write(ExportBitMask);
            if ((ExportBitMask & 64) != 0) CompressedInt.Write(w, TargetId);

            if ((ExportBitMask & 2) != 0) w.Write(PosA.X);

            if ((ExportBitMask & 4) != 0) w.Write(PosA.Y);

            if ((ExportBitMask & 8) != 0) w.Write(PosB.X);

            if ((ExportBitMask & 16) != 0) w.Write(PosB.Y);

            if ((ExportBitMask & 1) != 0) Color.Write(w);

            if ((ExportBitMask & 32) != 0) w.Write((float)Duration);

            if ((ExportBitMask & 128) != 0) w.Write(Size);
        }
    }
}