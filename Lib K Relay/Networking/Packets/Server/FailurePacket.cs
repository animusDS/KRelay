using Lib_K_Relay.Utilities;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class FailurePacket : Packet
    {
        /* Error IDs:
         * Unknown = 1
         * IncorrectVersion = 4
         * BadKey = 5
         * InvalidTeleportTarget = 6
         * EmailVerificationNeeded = 7
         * TeleportRealmBlock = 8
         * Unknown = 9
         * WrongServerEntered = 10
         * Unknown = 14
         * ServerQueueFull = 15
         * Unknown = 16
         */

        public int ErrorId;
        public string ErrorMessage;

        public override PacketType Type => PacketType.FAILURE;

        public override void Read(PacketReader r)
        {
            ErrorId = r.ReadInt32();
            ErrorMessage = r.ReadString();
            PluginUtils.Log("Packet", "Failure received: Id=" + ErrorId + ", Msg=" + ErrorMessage);
        }

        public override void Write(PacketWriter w)
        {
            w.Write(ErrorId);
            w.Write(ErrorMessage);
        }
    }
}