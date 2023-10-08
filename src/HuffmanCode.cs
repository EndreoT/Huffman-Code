using HuffmanCode.Extensions;
using System.Collections;
using System.Text;

namespace HuffmanCode;

public static class HuffmanCode
{
    /// <summary>
    /// Supports surrogate code points but not grapheme clusters
    /// See <see href="https://learn.microsoft.com/en-us/dotnet/standard/base-types/character-encoding-introduction"/>
    /// </summary>
    /// <param name="input"></param>
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

    private static void ValidateText(ReadOnlySpan<char> input)
    {
        foreach (Rune c in input.EnumerateRunes())
        {
            if (c.Value == Constants.PseudoEndOfFileChar)
            {
                throw new ArgumentException($"Input cannot contain the character: {Constants.PseudoEndOfFileChar}", nameof(input));
            }
        }
    }

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