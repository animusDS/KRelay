using Lib_K_Relay.Networking.Packets.DataObjects;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class VaultContentPacket : Packet
    {
        public short CurrentPotionMax;
        public int[] GiftContents;
        public int GiftItemCount;
        public byte[] GiftItemString;
        public short NextPotionMax;
        public int[] PotionContents;
        public int PotionItemCount;
        public short PotionUpgradeCost;
        public bool UnknownBool;
        public int[] VaultContents;
        public int VaultItemCount;
        public byte[] VaultItemString;
        public short VaultUpgradeCost;

        public override PacketType Type => PacketType.VAULT_CONTENT;

        public override void Read(PacketReader r)
        {
            UnknownBool = r.ReadBoolean();
            VaultItemCount = CompressedInt.Read(r);
            GiftItemCount = CompressedInt.Read(r);
            PotionItemCount = CompressedInt.Read(r);

            VaultContents = new int[CompressedInt.Read(r)];
            for (var i = 0; i < VaultContents.Length; i++)
                VaultContents[i] = CompressedInt.Read(r);

            GiftContents = new int[CompressedInt.Read(r)];
            for (var i = 0; i < GiftContents.Length; i++)
                GiftContents[i] = CompressedInt.Read(r);

            PotionContents = new int[CompressedInt.Read(r)];
            for (var i = 0; i < PotionContents.Length; i++)
                PotionContents[i] = CompressedInt.Read(r);

            VaultUpgradeCost = r.ReadInt16();
            PotionUpgradeCost = r.ReadInt16();
            CurrentPotionMax = r.ReadInt16();
            NextPotionMax = r.ReadInt16();
            VaultItemString = r.ReadBytes(r.ReadInt16());
            GiftItemString = r.ReadBytes(r.ReadInt16());
        }

        public override void Write(PacketWriter w)
        {
            w.Write(UnknownBool);
            CompressedInt.Write(w, VaultItemCount);
            CompressedInt.Write(w, GiftItemCount);
            CompressedInt.Write(w, PotionItemCount);

            CompressedInt.Write(w, VaultContents.Length);
            foreach (var i in VaultContents)
                CompressedInt.Write(w, i);

            CompressedInt.Write(w, GiftContents.Length);
            foreach (var i in GiftContents)
                CompressedInt.Write(w, i);

            CompressedInt.Write(w, PotionContents.Length);
            foreach (var i in PotionContents)
                CompressedInt.Write(w, i);

            w.Write(VaultUpgradeCost);
            w.Write(PotionUpgradeCost);
            w.Write(CurrentPotionMax);
            w.Write(NextPotionMax);

            w.Write((short)VaultItemString.Length);
            w.Write(VaultItemString);
            w.Write((short)GiftItemString.Length);
            w.Write(GiftItemString);
        }
    }
}