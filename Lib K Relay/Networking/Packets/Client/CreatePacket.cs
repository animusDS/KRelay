namespace Lib_K_Relay.Networking.Packets.Client
{
    public class CreatePacket : Packet
    {
        public ushort ClassType;
        public ushort SkinType;
        public bool Unknown;

        public override PacketType Type => PacketType.CREATE;

        public override void Read(PacketReader r)
        {
            ClassType = r.ReadUInt16();
            SkinType = r.ReadUInt16();
            Unknown = r.ReadBoolean();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(ClassType);
            w.Write(SkinType);
            w.Write(Unknown);
        }
    }
}