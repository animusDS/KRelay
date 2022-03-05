using Lib_K_Relay.Networking.Packets.DataObjects.Data;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class ForgeUnlockedBlueprintsPacket : Packet
    {
        public int[] UnlockedItems;

        public override PacketType Type => PacketType.FORGE_UNLOCKED_BLUEPRINTS;

        public override void Read(PacketReader r)
        {
            UnlockedItems = new int[r.ReadByte()];
            for (var i = 0; i < UnlockedItems.Length; i++)
                UnlockedItems[i] = CompressedInt.Read(r);
        }

        public override void Write(PacketWriter w)
        {
            w.Write((byte)UnlockedItems.Length);
            foreach (var item in UnlockedItems)
                CompressedInt.Write(w, item);
        }
    }
}