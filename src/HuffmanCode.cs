using HuffmanCode.Extensions;
using System.Collections;

namespace HuffmanCode;

public static class HuffmanCode
{
    public static Stream EncodeString(ReadOnlySpan<char> input)
    {
        if (input.Length == 0)
        {
            return Stream.Null;
        }
        ValidateText(input);

        Dictionary<char, int> charCount = Utils.BuildCharacterFrequencyMap(input);

        HuffmanTreeNode root = HuffmanTreeBuilder.BuildHuffmanTree(charCount);

        root.PrintBFS();

        Dictionary<char, BitArray> huffmanCode = Utils.BuildCharacterMapToHuffmanCode(root);

        PrintHuffmanCode(huffmanCode);

        BitArray bits = Utils.EncodeStringToBits(input, huffmanCode);

        string huffmanEncodingHeader = Utils.GetHuffmanEncodingHeader(charCount);

        return Utils.WriteHeaderAndDataToStream(GetBytes(bits), huffmanEncodingHeader);
    }

    private static void ValidateText(ReadOnlySpan<char> input)
    {
        foreach (char c in input)
        {
            if (c == Constants.PseudoEndOfFileChar)
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

        return Utils.DecodeToString(stream);
    }

    private static byte[] GetBytes(BitArray bits)
    {
        Console.WriteLine($"{bits.ToStringReversed()} : bit string after padding\n");

        byte[] bytes = bits.ToByteArray();

        Array.Reverse(bytes); // Reverse bytes so the stream during decoding is read in the correct order

        return bytes;
    }

    public static void PrintHuffmanCode(Dictionary<char, BitArray> huffmanCode)
    {
        foreach (KeyValuePair<char, BitArray> item in huffmanCode)
        {
            BitArray b = item.Value;
            string bitStr = b.ToStringReversed();
            Console.WriteLine($"{item.Key}, NumBits={item.Value.Count}, {bitStr}");
        }

        Console.WriteLine();
    }
}