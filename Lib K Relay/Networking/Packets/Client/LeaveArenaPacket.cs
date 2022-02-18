﻿namespace Lib_K_Relay.Networking.Packets.Client
{
    public class LeaveArenaPacket : Packet
    {
        public int Time;

        public override PacketType Type => PacketType.ARENA_DEATH;

        public override void Read(PacketReader r)
        {
            Time = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Time);
        }
    }
}