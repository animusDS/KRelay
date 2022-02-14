using Lib_K_Relay.Networking.Packets.DataObjects;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class VaultContentPacket : Packet
    {
        public short CurrentPotionMax;
        public int[] GiftContents;
        public short NextPotionMax;
        public int[] PotionContents;
        public short PotionUpgradeCost;
        public bool Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public byte[] Unknown5;
        public byte[] Unknown6;
        public int[] VaultContents;
        public short VaultUpgradeCost;

        public override PacketType Type => PacketType.VAULT_CONTENT;

        public override void Read(PacketReader r)
        {
            Unknown1 = r.ReadBoolean();
            Unknown2 = CompressedInt.Read(r);
            Unknown3 = CompressedInt.Read(r);
            Unknown4 = CompressedInt.Read(r);

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
            Unknown5 = r.ReadBytes(r.ReadInt16());
            Unknown6 = r.ReadBytes(r.ReadInt16());
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Unknown1);
            CompressedInt.Write(w, Unknown2);
            CompressedInt.Write(w, Unknown3);
            CompressedInt.Write(w, Unknown4);

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

            w.Write((short)Unknown5.Length);
            w.Write(Unknown5);
            w.Write((short)Unknown6.Length);
            w.Write(Unknown6);
        }
    }
}