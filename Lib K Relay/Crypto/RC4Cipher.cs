using System;
using System.Text;

namespace Lib_K_Relay.Crypto
{
    public class Rc4Cipher
    {
        private static readonly int StateLength = 256;

        private byte[] _engineState;
        private byte[] _workingKey;
        private int _x;
        private int _y;

        public Rc4Cipher(byte[] key)
        {
            _workingKey = key;
            SetKey(_workingKey);
        }

        public Rc4Cipher(string hexString)
        {
            _workingKey = HexStringToBytes(hexString);
            SetKey(_workingKey);
        }

        public void Cipher(byte[] packet)
        {
            ProcessBytes(packet, 5, packet.Length - 5, packet, 5);
        }

        public void Reset()
        {
            SetKey(_workingKey);
        }

        private void ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
        {
            /*
            if ((inOff + length) > input.Length)
                throw new ArgumentException("input buffer too short");

            if ((outOff + length) > output.Length)
                throw new ArgumentException("output buffer too short");
            */
            for (var i = 0; i < length; i++)
            {
                _x = (_x + 1) & 0xff;
                _y = (_engineState[_x] + _y) & 0xff;

                // swap
                var tmp = _engineState[_x];
                _engineState[_x] = _engineState[_y];
                _engineState[_y] = tmp;

                // xor
                output[i + outOff] = (byte)(input[i + inOff]
                                            ^ _engineState[(_engineState[_x] + _engineState[_y]) & 0xff]);
            }
        }

        private void SetKey(byte[] keyBytes)
        {
            _workingKey = keyBytes;
            _x = _y = 0;

            if (_engineState == null) _engineState = new byte[StateLength];

            // reset the state of the engine
            for (var i = 0; i < StateLength; i++) _engineState[i] = (byte)i;

            int i1 = 0, i2 = 0;

            for (var i = 0; i < StateLength; i++)
            {
                i2 = ((keyBytes[i1] & 0xff) + _engineState[i] + i2) & 0xff;
                // do the byte-swap inline
                var tmp = _engineState[i];
                _engineState[i] = _engineState[i2];
                _engineState[i2] = tmp;
                i1 = (i1 + 1) % keyBytes.Length;
            }
        }

        public static byte[] HexStringToBytes(string key)
        {
            if (key.Length % 2 != 0) throw new ArgumentException("Invalid hex string!");

            var bytes = new byte[key.Length / 2];
            var c = key.ToCharArray();
            for (var i = 0; i < c.Length; i += 2)
            {
                var sb = new StringBuilder(2).Append(c[i]).Append(c[i + 1]);
                var j = Convert.ToInt32(sb.ToString(), 16);
                bytes[i / 2] = (byte)j;
            }

            return bytes;
        }
    }
}