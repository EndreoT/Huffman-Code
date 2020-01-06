using System;
using System.Collections.Generic;
using System.Text;

namespace HuffmanCode.ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static void ReverseStringBuilder(this StringBuilder sb)
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
