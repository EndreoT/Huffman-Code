using System.Text;

namespace HuffmanCode.Extensions
{
    internal static class ReadOnlySpanExtensions
    {
        public static Dictionary<Rune, uint> BuildCharacterFrequencyMap(this ReadOnlySpan<char> input)
        {
            Dictionary<Rune, uint> charFrequency = new();
            foreach (Rune c in input.EnumerateRunes())
            {
                uint count = charFrequency.GetValueOrDefault<Rune, uint>(c, 0);
                charFrequency[c] = count + 1;
            }

            // Add pseudo EOF character
            charFrequency[new Rune(Constants.PseudoEndOfFileChar)] = 1;

            return charFrequency;
        }
    }
}