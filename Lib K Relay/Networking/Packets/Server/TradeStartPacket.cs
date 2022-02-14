using Lib_K_Relay.Networking.Packets.DataObjects;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class TradeStartPacket : Packet
    {
        public Item[] MyItems;
        public int PartnerObjectId;
        public Item[] YourItems;
        public string YourName;

        public override PacketType Type => PacketType.TRADESTART;

        public override void Read(PacketReader r)
        {
            MyItems = new Item[r.ReadInt16()];
            for (var i = 0; i < MyItems.Length; i++) MyItems[i] = (Item)new Item().Read(r);

            YourName = r.ReadString();
            YourItems = new Item[r.ReadInt16()];
            for (var i = 0; i < YourItems.Length; i++) YourItems[i] = (Item)new Item().Read(r);

            r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write((ushort)MyItems.Length);
            foreach (var i in MyItems) i.Write(w);

            w.Write(YourName);
            w.Write((ushort)YourItems.Length);
            foreach (var i in YourItems) i.Write(w);

            w.Write(PartnerObjectId);
        }
    }
}