using System.Collections;
using System.Text;

namespace HuffmanCode.Extensions
{
    internal static class BitArrayExtensions
    {
        public static string ToStringReversed(this BitArray bits)
        {
            StringBuilder stringBuilder = new(8);
            int index = 0;

            while (index < bits.Count)
            {
                bool bit = bits.Get(index);
                char bitChar = bit ? '1' : '0';
                stringBuilder.Append(bitChar);
                ++index;
            }
            stringBuilder.Reverse();

            return stringBuilder.ToString();
        }

        public static BitArray LeftShiftOnce(this BitArray bits)
        {
            ++bits.Length;
            bits.LeftShift(1);
            return bits;
        }

        public static BitArray LeftShiftOncePlusOne(this BitArray bits)
        {
            bits = LeftShiftOnce(bits);
            bits.Set(0, true);
            return bits;
        }

        public static byte[] ToByteArray(this BitArray bits)
        {
            if (bits.Length == 0)
            {
                return Array.Empty<byte>();
            }

            byte[] res = new byte[((bits.Length - 1) / 8) + 1];
            bits.CopyTo(res, 0);
            return res;
        }
    }
}