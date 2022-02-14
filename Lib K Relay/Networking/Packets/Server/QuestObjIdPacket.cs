using Lib_K_Relay.Networking.Packets.DataObjects;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class QuestObjIdPacket : Packet
    {
        public int ObjectId;
        public int[] Unknown;

        public override PacketType Type => PacketType.QUESTOBJID;

        public override void Read(PacketReader r)
        {
            ObjectId = r.ReadInt32();
            Unknown = new int[CompressedInt.Read(r)];
            for (var i = 0; i < Unknown.Length; i++) Unknown[i] = CompressedInt.Read(r);
        }

        public override void Write(PacketWriter w)
        {
            w.Write(ObjectId);
            CompressedInt.Write(w, Unknown.Length);
            foreach (var i in Unknown) CompressedInt.Write(w, i);
        }
    }
}