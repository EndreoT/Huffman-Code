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

        public static Dictionary<char, BitArray> BuildCharacterMapToHuffmanCode(TreeNode root)
        {
            Dictionary<char, BitArray> huffmanCode = new();
            Queue<TreeNode> queue = new();

            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                TreeNode node = queue.Dequeue();

                if (node.Left is not null)
                {
                    node.Left.HuffmanCode = node.GetHuffmanCodeCopy().ShiftLeftOncePlusOne();
                    queue.Enqueue(node.Left);
                }
                if (node.Right is not null)
                {
                    node.Right.HuffmanCode = node.GetHuffmanCodeCopy().ShiftLeftOnce();
                    queue.Enqueue(node.Right);
                }
                if (node.IsLeafNode())
                {
                    huffmanCode.Add(node.Character, node.GetHuffmanCodeCopy());
                }
            }
            return huffmanCode;
        }

        public static byte[] BitArrayToByteArray(BitArray bits)
        {
            if (bits.Length == 0)
            {
                return Array.Empty<byte>();
            }

            byte[] res = new byte[((bits.Length - 1) / 8) + 1];
            bits.CopyTo(res, 0);
            return res;
        }

        public static BitArray EncodeStringToBits(string str, Dictionary<char, BitArray> huffmanCode)
        {
            BitArray bitArray = new(0);
            foreach (char c in str)
            {
                if (huffmanCode.ContainsKey(c))
                {
                    //Make deep copy of item value
                    BitArray bitsToAdd = new(huffmanCode[c]);

                    int extend = bitsToAdd.Count;

                    bitArray.Length += extend;
                    bitArray.LeftShift(extend);

                    int currentLen = bitArray.Count;
                    bitsToAdd.Length += (currentLen - extend);

                    bitArray.Or(bitsToAdd);
                }
                else
                {
                    throw new KeyNotFoundException($"Character '{c}' does not exist.");
                }
            }

            return bitArray;
        }

        public static StringBuilder DecodeToString(Stream stream, int numBitsToRead, TreeNode root)
        {
            if (root == null)
            {
                throw new Exception("node is null");
            }
            StringBuilder stringBuilder = new();

            stream.Position = 0;
            using BinaryReader reader = new(stream);

            byte[] buffer = reader.ReadBytes((int)stream.Length);
            Array.Reverse(buffer, 0, buffer.Length);

            //foreach (byte b in buffer)
            //{
            //    Console.Write(Convert.ToString(b, 2).PadLeft(8, '0'));
            //}
            //Console.WriteLine();

            stream.Position = 0;
            TreeNode? node = root;
            for (int i = 0; i < (int)stream.Length; i++)
            {
                try
                {
                    byte byteRead = buffer[i];

                    int offsetBit = 0;
                    if (i == 0)
                    {
                        // First byte may not use all bits
                        offsetBit = 8 - (numBitsToRead % 8);
                    }

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

                        if (node.IsLeafNode())
                        {
                            stringBuilder.Append(node.Character);
                            node = root;
                        }
                    }
                }
                catch (EndOfStreamException)
                {
                    throw;
                }
            }

            return stringBuilder;
        }
    }
}