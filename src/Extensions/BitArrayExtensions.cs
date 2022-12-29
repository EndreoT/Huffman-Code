using System.Collections;
using System.Text;

namespace HuffmanCode.Extensions
{
    public static class BitArrayExtensions
    {
        public static string ToStringReversed(this BitArray bitArray)
        {
            StringBuilder stringBuilder = new(8);
            int index = 0;

            while (index < bitArray.Count)
            {
                bool bit = bitArray.Get(index);
                char bitChar = bit ? '1' : '0';
                stringBuilder.Append(bitChar);
                ++index;
            }
            stringBuilder.Reverse();

            return stringBuilder.ToString();
        }

        public static BitArray ShiftLeftOnce(this BitArray currentHFCode)
        {
            ++currentHFCode.Length;
            currentHFCode.LeftShift(1);
            return currentHFCode;
        }

        public static BitArray ShiftLeftOncePlusOne(this BitArray currentHFCode)
        {
            ++currentHFCode.Length;
            currentHFCode.LeftShift(1);
            currentHFCode.Set(0, true);
            return currentHFCode;
        }
    }
}