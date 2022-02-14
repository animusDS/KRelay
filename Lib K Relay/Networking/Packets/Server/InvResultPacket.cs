using Lib_K_Relay.Networking.Packets.DataObjects;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class InvResultPacket : Packet
    {
        public SlotObject FromSlotObject = new SlotObject();
        public bool Result;
        public SlotObject ToSlotObject = new SlotObject();

        public override PacketType Type => PacketType.INVRESULT;

        public override void Read(PacketReader r)
        {
            Result = r.ReadBoolean();
            FromSlotObject.Read(r);
            ToSlotObject.Read(r);
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Result);
            FromSlotObject.Write(w);
            ToSlotObject.Write(w);
        }
    }
}