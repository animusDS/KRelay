﻿namespace Lib_K_Relay.Networking.Packets.Server
{
    public class NameResultPacket : Packet
    {
        public string ErrorText;
        public bool Success;

        public override PacketType Type => PacketType.NAMERESULT;

        public override void Read(PacketReader r)
        {
            Success = r.ReadBoolean();
            ErrorText = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Success);
            w.Write(ErrorText);
        }
    }
}