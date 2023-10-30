using HuffmanCode.Extensions;
using System.Collections;
using System.Text;

namespace HuffmanCode;

public static class HuffmanCode
{
    /// <summary>
    /// Encodes a <see cref="ReadOnlySpan{Char}"/> to a <see cref="Stream"/> using Huffman encoding. This function
    /// supports valid surrogate code points, but does not support invalid surrogate code points (invalid pairs, etc.)
    /// and not grapheme clusters. For more info on character encoding, see <see href="https://learn.microsoft.com/en-us/dotnet/standard/base-types/character-encoding-introduction"/>.
    /// To learn more about Huffman encoding, see <see href="https://web.stanford.edu/class/archive/cs/cs106b/cs106b.1176/assnFiles/assign6/huffman-encoding-supplement.pdf"/>
    /// </summary>
    /// <param name="input">The input to be encoded</param>
    /// <returns></returns>
    public static Stream EncodeString(ReadOnlySpan<char> input)
    {
        if (input.Length == 0)
        {
            return Stream.Null;
        }
        ValidateText(input);

        Dictionary<Rune, uint> charFrequency = input.BuildCharacterFrequencyMap();

        HuffmanTreeNode root = HuffmanTreeBuilder.BuildHuffmanTree(charFrequency);

        root.PrintBFS();

        Dictionary<Rune, BitArray> huffmanCode = root.BuildCharacterMapToHuffmanCode();

        PrintHuffmanCode(huffmanCode);

        BitArray bits = BitEncoder.EncodeInputToBits(input, huffmanCode);

        string huffmanEncodingHeader = Utils.GetHuffmanEncodingHeader(charFrequency);

        return Utils.WriteHeaderAndDataToStream(GetBytes(bits), huffmanEncodingHeader);
    }

    private static void ValidateText(ReadOnlySpan<char> input) // TODO validate if valid surrogates
    {
        foreach (Rune c in input.EnumerateRunes())
        {
            if (c.Value == Constants.PseudoEndOfFileChar)
            {
                throw new ArgumentException($"Input cannot contain the character: {Constants.PseudoEndOfFileChar}", nameof(input));
            }
        }
    }

    /// <summary>
    /// Decodes a Huffman encoded <see cref="Stream"/> back to a human readable string text
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string DecodeToString(Stream stream)
    {
        stream = stream ?? throw new ArgumentNullException(nameof(stream));
        if (stream == Stream.Null || stream.Length == 0)
        {
            return "";
        }

        return Utils.DecodeHuffmanEncodingToString(stream);
    }

    private static byte[] GetBytes(BitArray bits)
    {
        Console.WriteLine($"{bits.ToStringReversed()} : bit string after padding\n");

        byte[] bytes = bits.ToByteArray();

        Array.Reverse(bytes); // Reverse bytes so the stream during decoding is read in the correct order

        return bytes;
    }

    public static void PrintHuffmanCode(Dictionary<Rune, BitArray> huffmanCode)
    {
        foreach (KeyValuePair<Rune, BitArray> item in huffmanCode)
        {
            BitArray b = item.Value;
            string bitStr = b.ToStringReversed();
            Console.WriteLine($"{item.Key}, NumBits={item.Value.Count}, {bitStr}");
        }

        Console.WriteLine();
    }
}