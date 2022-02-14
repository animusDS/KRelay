using Lib_K_Relay.Utilities;

namespace Lib_K_Relay.Networking.Packets
{
    public class UndefinedPacket : Packet
    {
        public byte[] Bytes;

        public override PacketType Type => PacketType.UNDEFINED;

        public override void Read(PacketReader r)
        {
            var bytesAvailable = r.BaseStream.Length - 5;
            var msg = "Packet is not defined: " +
                      "Id=" + Id + ", Bytes=[";
            Bytes = new byte[bytesAvailable];
            for (var i = 0; i < bytesAvailable; i++)
            {
                Bytes[i] = r.ReadByte();
                msg += Bytes[i] + (i == bytesAvailable - 1 ? "]" : ",");
            }

            PluginUtils.Log("Packet", msg);
        }

        public override void Write(PacketWriter w)
        {
            foreach (var b in Bytes)
                w.Write(b);
        }
    }
}