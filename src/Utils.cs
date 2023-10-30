using System.Text;

namespace HuffmanCode;

internal static class Utils
{
    public static string DecodeHuffmanEncodingToString(Stream stream)
    {
        StringBuilder stringBuilder = new();

        stream.Position = 0;
        using BinaryReader reader = new(stream);
        int numBytesInHeader = reader.ReadInt32(); // Read header byte size

        byte[] huffmanEncoding = reader.ReadBytes(numBytesInHeader);
        string str = Encoding.UTF8.GetString(huffmanEncoding);

        Dictionary<Rune, uint> charFrequency = BuildCharFrequencyFromHuffmanStringEncoding(str);

        HuffmanTreeNode root = HuffmanTreeBuilder.BuildHuffmanTree(charFrequency);

        HuffmanTreeNode? node = root;
        int numBytesToRead = (int)stream.Length - numBytesInHeader - sizeof(int);
        for (int i = 0; i < numBytesToRead; i++)
        {
            byte byteRead = reader.ReadByte();

            int offsetBit = 0;

            for (int j = offsetBit; j < 8; j++)
            {
                int bit = (byteRead >> (7 - j)) & 1;

                if (bit == 0)
                {
                    node = node.Right;
                }
                else
                {
                    node = node.Left;
                }

                if (node is null)
                {
                    throw new Exception("node is null");
                }

                if (node.IsPseudoEOFCharacter())
                {
                    // Done with decoding. Ignore all remaining bits
                    break;
                }

                if (node.IsLeafNode())
                {
                    stringBuilder.Append(node.Character);
                    node = root;
                }
            }
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Encoding header example: 'A2 B11 C235 '
    /// </summary>
    /// <param name="header"></param>
    /// <returns></returns>
    public static Dictionary<Rune, uint> BuildCharFrequencyFromHuffmanStringEncoding(ReadOnlySpan<char> header)
    {
        Dictionary<Rune, uint> charFrequency = new();
        bool isPairStart = false;
        Rune currentChar = default;
        List<Rune> charCountDigits = new();

        foreach (Rune s in header.EnumerateRunes())
        {
            if (!isPairStart)
            {
                // Start a new character count pair
                currentChar = s;
                charCountDigits.Clear();
                isPairStart = true;
                continue;
            }

            if (Rune.IsDigit(s))
            {
                charCountDigits.Add(s);
            }
            else
            {
                // Reached a character count pair termination delimeter
                uint count = ParseDigitListToInt(charCountDigits);
                charFrequency[currentChar] = count;
                isPairStart = false;
            }
        }

        return charFrequency;
    }

    public static uint ParseDigitListToInt(IReadOnlyList<Rune> digits)
    {
        uint count = 0;
        int place = 1;
        for (int i = digits.Count - 1; i >= 0; i--)
        {
            int digit = Convert.ToInt32(digits[i].Value - 48); // 48 is ascii 0
            count += (uint)(digit * place);
            place *= 10;
        }

        return count;
    }

    public static string GetHuffmanEncodingHeader(Dictionary<Rune, uint> charFrequency)
    {
        StringBuilder sb = new();
        foreach (KeyValuePair<Rune, uint> kv in charFrequency)
        {
            sb.Append(kv.Key);
            sb.Append(kv.Value);
            sb.Append(' '); // Add delimeter
        }
        return sb.ToString();
    }

    public static Stream WriteHeaderAndDataToStream(byte[] bytes, string huffmanEncodingHeader)
    {
        Stream stream = new MemoryStream();
        using BinaryWriter writer = new(stream, Encoding.UTF8, true);

        // Write encoding
        int numBytesForEncoding = Encoding.UTF8.GetByteCount(huffmanEncodingHeader);
        byte[] headerBytes = Encoding.UTF8.GetBytes(huffmanEncodingHeader);
        writer.Write(numBytesForEncoding);

        writer.Write(headerBytes);

        // Write payload
        writer.Write(bytes);

        return stream;
    }
}