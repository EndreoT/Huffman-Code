using HuffmanCode.Extensions;
using System.Collections;
using System.Text;

namespace HuffmanCode;

public class HuffmanCode : IHuffmanCode
{
    public Stream EncodeString(string str)
    {
        ValidateText(str);

        str += Constants.PseudoEndOfFileChar;

        Dictionary<char, int> charCount = Utils.BuildCharacterFrequencyMap(str);

        HuffmanTreeNode root = HuffmanTreeBuilder.BuildHuffmanTree(charCount);

        Dictionary<char, BitArray> huffmanCode = Utils.BuildCharacterMapToHuffmanCode(root);

        PrintHuffmanCode(huffmanCode);

        Stream stream = new MemoryStream();
        using BinaryWriter writer = new(stream, Encoding.UTF8, true);

        // Write encoding header
        string huffmanEncodingHeader = GetEncodingHeaderString(charCount);
        int numBytesForEncoding = Encoding.UTF8.GetByteCount(huffmanEncodingHeader);
        writer.Write(numBytesForEncoding);
        writer.Write(huffmanEncodingHeader);

        // Write data
        BitArray bits = Utils.EncodeStringToBits(str, huffmanCode);
        writer.Write(GetBytes(bits));

        root.PrintBFS();

        return stream;
    }

    public string DecodeToString(Stream stream)
    {
        stream = stream ?? throw new ArgumentNullException(nameof(stream));

        return Utils.DecodeToString(stream);
    }

    private static string GetEncodingHeaderString(Dictionary<char, int> charCount)
    {
        StringBuilder sb = new();
        foreach (KeyValuePair<char, int> kv in charCount)
        {
            sb.Append($"{kv.Key}{kv.Value}");
            sb.Append(' ');
        }
        return sb.ToString();
    }

    private static byte[] GetBytes(BitArray bits)
    {
        Console.WriteLine($"{bits.ToStringReversed()} : bit string after padding\n");

        byte[] bytes = bits.ToByteArray();

        Array.Reverse(bytes); // Reverse bytes so the stream during decoding is read in the correct order

        return bytes;
    }

    private static void ValidateText(string str)
    {
        if (str is null)
        {
            throw new ArgumentNullException(nameof(str));
        }
        if (str[str.Length - 1] == Constants.PseudoEndOfFileChar)
        {
            throw new ArgumentException($"Text cannot end with the {Constants.PseudoEndOfFileChar} character", nameof(str));
        }
    }

    public void PrintHuffmanCode(Dictionary<char, BitArray> huffmanCode)
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