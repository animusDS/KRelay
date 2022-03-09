namespace Lib_K_Relay.Networking.Packets.DataObjects.Data
{
    internal class CompressedInt
    {
        public static int Read(PacketReader r)
        {
            var uByte = r.ReadByte();
            var isNegative = (uByte & 64) != 0;
            var shift = 6;
            var value = uByte & 63;

            while ((uByte & 128) != 0)
            {
                uByte = r.ReadByte();
                value |= (uByte & 127) << shift;
                shift += 7;
            }

            return isNegative ? -value : value;
        }

        public static void Write(PacketWriter w, int value)
        {
            if (value < 0)
            {
                value = -value;
                w.Write((byte)(64 | value));
            }
            else
            {
                w.Write((byte)value);
            }
        }
    }
}