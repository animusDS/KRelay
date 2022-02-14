namespace Lib_K_Relay.Networking.Packets.Server
{
    public class NewCharacterInformationPacket : Packet
    {
        public string CharacterXML;

        public override PacketType Type => PacketType.NEW_CHARACTER_INFORMATION;

        public override void Read(PacketReader r)
        {
            CharacterXML = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(CharacterXML);
        }
    }
}