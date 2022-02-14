using Lib_K_Relay.Networking.Packets.DataObjects;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class NewTickPacket : Packet
    {
        public ushort ServerLastRTTMS;
        public int ServerRealTimeMS;
        public Status[] Statuses;
        public int TickId;
        public int TickTime;

        public override PacketType Type => PacketType.NEWTICK;

        public override void Read(PacketReader r)
        {
            TickId = r.ReadInt32();
            TickTime = r.ReadInt32();
            ServerRealTimeMS = r.ReadInt32();
            ServerLastRTTMS = r.ReadUInt16();

            Statuses = new Status[r.ReadInt16()];
            for (var i = 0; i < Statuses.Length; i++) Statuses[i] = (Status)new Status().Read(r);
        }

        public override void Write(PacketWriter w)
        {
            w.Write(TickId);
            w.Write(TickTime);
            w.Write(ServerRealTimeMS);
            w.Write(ServerLastRTTMS);

            w.Write((short)Statuses.Length);
            foreach (var s in Statuses) s.Write(w);
        }
    }
}