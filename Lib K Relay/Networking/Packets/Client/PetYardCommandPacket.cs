using Lib_K_Relay.Networking.Packets.DataObjects;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class PetYardCommandPacket : Packet
    {
        public const int UpgradePetYard = 1;
        public const int FeedPet = 2;
        public const int FusePet = 3;

        public byte CommandId;
        public byte Currency;
        public int ObjectId;
        public SlotObject ObjectSlot;
        public int PetId1;
        public int PetId2;

        public override PacketType Type => PacketType.PETUPGRADEREQUEST;

        public override void Read(PacketReader r)
        {
            CommandId = r.ReadByte();
            PetId1 = r.ReadInt32();
            PetId2 = r.ReadInt32();
            ObjectId = r.ReadInt32();
            ObjectSlot = (SlotObject)new SlotObject().Read(r);
            Currency = r.ReadByte();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(CommandId);
            w.Write(PetId1);
            w.Write(PetId2);
            w.Write(ObjectId);
            ObjectSlot.Write(w);
            w.Write(Currency);
        }
    }
}