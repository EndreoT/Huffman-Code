using HuffmanCode.Extensions;
using System.Collections;
using System.Text;

namespace HuffmanCode
{
    internal static class Utils
    {
        public static Dictionary<char, int> BuildCharacterFrequencyMap(string str)
        {
            Dictionary<char, int> charCount = new();
            foreach (char c in str)
            {
                if (charCount.ContainsKey(c))
                {
                    charCount[c] += 1;
                }
                else
                {
                    charCount[c] = 1;
                }
            }
            return charCount;
        }

        public static Dictionary<char, BitArray> BuildCharacterMapToHuffmanCode(HuffmanTreeNode root)
        {
            Dictionary<char, BitArray> huffmanCode = new();
            Queue<HuffmanTreeNode> queue = new();

            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                HuffmanTreeNode node = queue.Dequeue();

                if (node.Left is not null)
                {
                    node.Left.HuffmanCode = node.GetHuffmanCodeCopy().LeftShiftOncePlusOne();
                    queue.Enqueue(node.Left);
                }
                if (node.Right is not null)
                {
                    node.Right.HuffmanCode = node.GetHuffmanCodeCopy().LeftShiftOnce();
                    queue.Enqueue(node.Right);
                }
                if (node.IsLeafNode())
                {
                    huffmanCode.Add(node.Character, node.GetHuffmanCodeCopy());
                }
            }
            return huffmanCode;
        }

        public static BitArray EncodeStringToBits(string str, Dictionary<char, BitArray> huffmanCode)
        {
            BitArray bitArray = new(0);
            foreach (char c in str)
            {
                if (!huffmanCode.TryGetValue(c, out BitArray? bitsToAdd))
                {
                    throw new KeyNotFoundException($"Character '{c}' does not exist in huffman code.");
                }

                int numBitsToAdd = bitsToAdd.Count;

                bitArray.Length += numBitsToAdd;
                bitArray.LeftShift(numBitsToAdd);

                for (int i = numBitsToAdd - 1; i >= 0; i--)
                {
                    bitArray[i] = bitsToAdd[i];
                }
            }

            // Add padding to reach a full byte if needed
            int numBitsInLastByte = bitArray.Length % 8;
            if (numBitsInLastByte > 0)
            {
                int shiftLen = 8 - numBitsInLastByte;
                bitArray.Length += shiftLen;
                bitArray.LeftShift(shiftLen);
            }

            return bitArray;
        }

        public static string DecodeToString(Stream stream)
        {
            stream = stream ?? throw new ArgumentNullException(nameof(stream));

            StringBuilder stringBuilder = new();

            stream.Position = 0;
            using BinaryReader reader = new(stream);
            int numBytesInHeader = reader.ReadInt32(); // Read header byte size

            numBytesInHeader += 1; // Account for space between encoding and data

            byte[] huffmanEncoding = reader.ReadBytes(numBytesInHeader);
            var str = Encoding.UTF8.GetString(huffmanEncoding);
            str = str[1..]; // TODO what is this first char?

            Dictionary<char, int> charCount = GetCharCountFromHuffmanStringEncoding(str);

            HuffmanTreeNode root = HuffmanTreeBuilder.BuildHuffmanTree(charCount);

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
        /// Encoding header example: 'A0 B11 C235 '
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Dictionary<char, int> GetCharCountFromHuffmanStringEncoding(string str)
        {
            Dictionary<char, int> charCount = new();
            bool isPairStart = false;
            char currentChar = '\0';
            List<char> charCountDigits = new();

            foreach (char s in str)
            {
                if (!isPairStart)
                {
                    // Start a new character count pair
                    currentChar = s;
                    charCountDigits.Clear();
                    isPairStart = true;
                    continue;
                }

                if (char.IsDigit(s))
                {
                    charCountDigits.Add(s);
                }
                else
                {
                    // Reached a character count pair termination delimeter
                    int count = ParseDigitListToInt(charCountDigits);
                    charCount[currentChar] = count;
                    isPairStart = false;
                }
            }

            return charCount;
        }

        public static int ParseDigitListToInt(IList<char> charCountDigits)
        {
            int count = 0;
            int place = 1;
            for (int i = charCountDigits.Count - 1; i >= 0; i--)
            {
                int digit = int.Parse(charCountDigits[i].ToString());
                count += digit * place;
                place *= 10;
            }

            return count;
        }
    }
}