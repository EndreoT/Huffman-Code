using System.Text;

namespace HuffmanCode.Extensions
{
    internal static class StringBuilderExtensions
    {
        public static void Reverse(this StringBuilder sb)
        {
            if (sb.Length == 0)
            {
                return;
            }
            char temp;
            int left = 0;
            int right = sb.Length - 1;
            while (left < right)
            {
                temp = sb[left];
                sb[left] = sb[right];
                sb[right] = temp;
                ++left;
                --right;
            }
        }
    }
}