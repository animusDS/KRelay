namespace Lib_K_Relay.Networking.Packets.DataObjects
{
    internal class CompressedInt
    {
        public static int Read(PacketReader r)
        {
            int ret;
            var trail = r.ReadByte();
            var above64 = (trail & 64) != 0;
            var shift = 6;
            ret = trail & 63;

            while ((trail & 128) != 0)
            {
                trail = r.ReadByte();
                ret |= (trail & 127) << shift;
                shift += 7;
            }

            if (above64)
                ret = -ret;

            return ret;
        }

        public static void Write(PacketWriter w, int value)
        {
            var num = value < 0;
            var num2 = (uint)(num ? -value : value);
            var b = (byte)(num2 & 0x3Fu);
            if (num) goto IL_0018;
            goto IL_00dd;
            IL_001d:
            int num3;
            var flag = default(bool);
            while (true)
            {
                uint num4;
                switch ((num4 = (uint)num3 ^ 0xA6922C20u) % 14u)
                {
                    case 12u:
                        break;
                    default:
                        return;
                    case 1u:
                        b = (byte)(b | 0x40u);
                        num3 = ((int)num4 * -2026352417) ^ -492387462;
                        continue;
                    case 11u:
                        flag = num2 != 0;
                        num3 = (int)(num4 * 2004575782) ^ -1876036982;
                        continue;
                    case 13u:
                        b = (byte)(num2 & 0x7Fu);
                        num3 = -835364367;
                        continue;
                    case 9u:
                        num2 >>= 7;
                        num3 = ((int)num4 * -517822387) ^ 0xB681355;
                        continue;
                    case 4u:
                    {
                        flag = num2 != 0;
                        int num7;
                        int num8;
                        if (flag)
                        {
                            num7 = 1784418546;
                            num8 = num7;
                        }
                        else
                        {
                            num7 = 1607897544;
                            num8 = num7;
                        }

                        num3 = num7 ^ ((int)num4 * -1685987627);
                        continue;
                    }
                    case 3u:
                        goto IL_00dd;
                    case 8u:
                        goto IL_00eb;
                    case 0u:
                        b = (byte)(b | 0x80u);
                        num3 = (int)((num4 * 1553222280) ^ 0x30B49BD0);
                        continue;
                    case 2u:
                    {
                        int num5;
                        int num6;
                        if (flag)
                        {
                            num5 = 8112247;
                            num6 = num5;
                        }
                        else
                        {
                            num5 = 139491927;
                            num6 = num5;
                        }

                        num3 = num5 ^ (int)(num4 * 426145393);
                        continue;
                    }
                    case 10u:
                        w.Write(b);
                        num3 = -2071375380;
                        continue;
                    case 7u:
                        b = (byte)(b | 0x80u);
                        num3 = ((int)num4 * -157612073) ^ 0x547A06E6;
                        continue;
                    case 5u:
                        w.Write(b);
                        num3 = -2071375380;
                        continue;
                    case 6u:
                        return;
                }

                break;
                IL_00eb:
                int num9;
                if (!flag)
                {
                    num3 = -1887521864;
                    num9 = num3;
                }
                else
                {
                    num3 = -2140420195;
                    num9 = num3;
                }
            }

            IL_0018:
            num3 = -620070475;
            goto IL_001d;
            IL_00dd:
            num2 >>= 6;
            num3 = -380097459;
            goto IL_001d;
        }
    }
}