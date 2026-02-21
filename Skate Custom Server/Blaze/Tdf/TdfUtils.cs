using System.Text.RegularExpressions;

namespace Blaze
{
    public static class TdfUtils
    {
        public static ushort GetBodyLengthFromPacket(byte[] packetBytes)
        {
            try
            {
                return (ushort)((packetBytes[0] << 8) | packetBytes[1]);
            }
            catch
            {
                return 0;
            }
        }

        public static ushort GetCommandFromPacket(byte[] packetBytes)
        {
            try
            {
                return (ushort)((packetBytes[4] << 8) | packetBytes[5]);
            }
            catch
            {
                return 0;
            }
        }

        public static ushort GetComponentFromPacket(byte[] packetBytes)
        {
            try
            {
                return (ushort)((packetBytes[2] << 8) | packetBytes[3]);
            }
            catch
            {
                return 0;
            }
        }

        public static ushort GetMessageIDFromPacket(byte[] packetBytes)
        {
            try
            {
                return (ushort)((packetBytes[10] << 8) | packetBytes[11]);
            }
            catch
            {
                return 0;
            }
        }

        public static (int length, int bytesUsed) ReadTdfLengthBigEndian(Stream stream)
        {
            int b;
            List<byte> bytes = new List<byte>();

            do
            {
                b = stream.ReadByte();
                if (b == -1)
                    throw new EndOfStreamException("Unexpected end of stream while reading varint length.");

                bytes.Add((byte)b);
            }
            while ((b & 0x80) != 0);

            int value = 0;
            int shift = 0;

            for (int i = bytes.Count - 1; i >= 0; i--)
            {
                byte payload;
                if (i == 0)
                    payload = (byte)(bytes[i] & 0x3F); // First byte: 6 bits
                else
                    payload = (byte)(bytes[i] & 0x7F); // Other bytes: 7 bits

                value |= payload << shift;
                shift += (i == 0) ? 6 : 7;
            }

            return (value, bytes.Count);
        }

        

        public static List<byte> EncodeVlq(int value)
        {
            // Matches ReadTdfLengthBigEndian:
            // - First byte: 6 data bits (bit 6..1), MSB used as continuation
            // - Subsequent bytes: 7 data bits, MSB used as continuation
            var parts = new List<byte>();
            int v = value;

            // Take 7-bit chunks until <= 6 bits remain
            while (v >= (1 << 6))
            {
                parts.Add((byte)(v & 0x7F)); // 7 LSBs
                v >>= 7;
            }

            byte top = (byte)v; // <= 63 (6 bits)
            int m = parts.Count;
            int n = m + 1;
            var encoded = new byte[n];

            // First (most significant) chunk: 6 bits
            encoded[0] = (byte)(top & 0x3F);

            // Remaining chunks: 7 bits, from most-significant to least
            for (int k = 0; k < m; k++)
            {
                encoded[k + 1] = (byte)(parts[m - 1 - k] & 0x7F);
            }

            // Set continuation bit on all but last
            for (int i = 0; i < n - 1; i++)
            {
                encoded[i] |= 0x80;
            }

            return encoded.ToList();
        }


        public static (byte highPart, byte lowPart) SplitByte(byte value)
        {
            byte highPart = (byte)((value & 0xF0) >> 4); // Extract and shift upper nibble
            byte lowPart = (byte)(value & 0x0F);         // Extract lower nibble
            return (highPart, lowPart);
        }
        public static byte[] BuildTdfHeader(TdfType type, int length)
        {
            if (type == TdfType.Struct)
                return new byte[] { 0x00 };
            if (type == TdfType.Array)
                return new byte[] { 0xA1 };
            if (type == TdfType.Union)
                return new byte[] { 0xD0 };
            if (length <= 0x0E)
            {
                byte header = (byte)(((byte)type << 4) | length);
                return new byte[] { header };
            }
            else
            {
                byte header = (byte)(((byte)type << 4) | 0x0F);
                byte[] vlq = EncodeVlq(length).ToArray();
                byte[] result = new byte[1 + vlq.Length];
                result[0] = header;
                Buffer.BlockCopy(vlq, 0, result, 1, vlq.Length);
                return result;
            }
        }

        public static byte[] EncodeTdfTag(string tag)
        {
            //Storing a 4 character string in 3 bytes
            var tagValue = 0;

            for (var i = 0; i < tag.Length; i++)
            {
                tagValue |= (0x20 | (tag[i] & 0x1F)) << (3 - i) * 6;
            }

            var tagBytes = BitConverter.GetBytes(Convert.ToUInt32(tagValue));
            //shrink and convert endianness
            Array.Resize(ref tagBytes, 3);
            Array.Reverse(tagBytes);

            return tagBytes;
        }

        public static string DecodeTdfTag(byte[] bytes)
        {
            string label = "";
            byte[] tagBytes = new byte[4];
            tagBytes[0] = bytes[0];
            tagBytes[1] = bytes[1];
            tagBytes[2] = bytes[2];

            Array.Reverse(tagBytes);
            var tag = BitConverter.ToUInt32(tagBytes, 0) >> 8;

            //convert to string
            for (var i = 0; i < tagBytes.Length; ++i)
            {
                var val = (tag >> ((3 - i) * 6)) & 0x3F;
                if (val > 0)
                {
                    label += Convert.ToChar(0x40 | (val & 0x1F));
                }
            }

            //cleanup
            label = Regex.Replace(label, "[^A-Z]+", "");

            return label;
        }

        public static byte[] SwapEndianess(byte[] bytes)
        {
            byte[] finalBytes = (byte[])bytes.Clone();
            if (BitConverter.IsLittleEndian) Array.Reverse(finalBytes);
            return finalBytes;
        }

        public static byte[] SwapEndianess(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] SwapEndianess(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] SwapEndianess(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] SwapEndianess(uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] SwapEndianess(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }

        public static byte[] SwapEndianess(ulong value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes;
        }
    }
}
