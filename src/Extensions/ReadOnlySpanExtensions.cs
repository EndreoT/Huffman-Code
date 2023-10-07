using System.Text;

namespace HuffmanCode.Extensions
{
    internal static class ReadOnlySpanExtensions
    {
        public static Dictionary<Rune, int> BuildCharacterFrequencyMap(this ReadOnlySpan<char> input)
        {
            Dictionary<Rune, int> charFrequency = new();
            foreach (Rune c in input.EnumerateRunes())
            {
                int count = charFrequency.GetValueOrDefault(c, 1);
                charFrequency[c] = count;
            }

            // Add pseudo EOF character
            charFrequency[new Rune(Constants.PseudoEndOfFileChar)] = 1;

            return charFrequency;
        }
    }
}