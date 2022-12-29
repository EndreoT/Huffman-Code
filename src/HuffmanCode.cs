using HuffmanCode.Extensions;
using System.Collections;
using System.Text;

namespace HuffmanCode;

public class HuffmanCode
{
    public static EncodedStringContext EncodeString(string str)
    {
        if (str is null)
        {
            throw new ArgumentNullException(nameof(str));
        }

        Dictionary<char, int> charCount = Utils.BuildCharacterFrequencyMap(str);

        TreeNode root = HuffmanTreeBuilder.BuildHuffmanTree(charCount);

        Dictionary<char, BitArray> huffmanCode = Utils.BuildCharacterMapToHuffmanCode(root);

        PrintHuffmanCode(huffmanCode);

        BitArray bits = Utils.EncodeStringToBits(str, huffmanCode);

        Console.WriteLine($"{bits.ToStringReversed()} : actual bit string\n");

        Stream stream = new MemoryStream();

        using BinaryWriter writer = new(stream, Encoding.UTF8, true);

        writer.Write(Utils.BitArrayToByteArray(bits));

        return new EncodedStringContext(stream, bits.Length, root);
    }

    public static string DecodeToString(Stream stream, int numBitsToRead, TreeNode? root)
    {
        if (stream is null || numBitsToRead <= 0 || root is null)
        {
            throw new ArgumentException("");
        }

        return Utils.DecodeToString(stream, numBitsToRead, root).ToString();
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