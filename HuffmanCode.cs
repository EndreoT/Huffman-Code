using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using HuffmanCode.ExtensionMethods;

using Priority_Queue;

namespace HuffmanCode
{
    class HuffmanCode
    {
        public static Tuple<BitArray, TreeNode?> EncodeString(string str)
        {
            Dictionary<char, int> charCount = BuildFrequencyMap(str);

            SimplePriorityQueue<TreeNode> heap = BuildPriorityQueue(charCount);

            TreeNode? tree = BuildHuffmanTree(heap);

            TreeNode.PrintTreeBFS(tree);
            Console.WriteLine();

            Dictionary<char, BitArray> huffmanCode = BuildHuffmanCode(tree);

            PrintHuffmanCode(huffmanCode);
            Console.WriteLine();

            BitArray concatenatedBits = EncodeStringToBits(str, huffmanCode);

            Console.WriteLine(BitArrayFullString(concatenatedBits));
            Console.WriteLine();

            return new Tuple<BitArray, TreeNode?>(concatenatedBits, tree);
        }

        public static string DecodeString(BitArray bits, TreeNode? huffmanCode)
        {
            return DecodeBitsToString(bits, huffmanCode).ToString();
        } 

        public static void PrintHuffmanCode(Dictionary<char, BitArray> huffmanCode)
        {
            foreach (KeyValuePair<char, BitArray> item in huffmanCode)
            {
                string bitStr = BitArrayFullString(item.Value);
                Console.WriteLine(String.Format("{0}, NumBits={1}, {2}", item.Key.ToString(), item.Value.Count, bitStr));
            }
        }

        

        public static string BitArrayFullString(BitArray bitArray)
        {
            StringBuilder stringBuilder = new StringBuilder(8);
            int index = 0;
            bool bit;
            char bitChar;
            while (index < bitArray.Count)
            {
                bit = bitArray.Get(index);
                bitChar = bit ? '1' : '0';
                stringBuilder.Append(bitChar);
                ++index;
            }
            stringBuilder.ReverseStringBuilder();

            return stringBuilder.ToString();

        }

        private static Dictionary<char, int> BuildFrequencyMap(string str)
        {
            Dictionary<char, int> charCount = new Dictionary<char, int>();
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
        private static SimplePriorityQueue<TreeNode> BuildPriorityQueue(Dictionary<char, int> charMap)
        {
            SimplePriorityQueue<TreeNode> heap = new SimplePriorityQueue<TreeNode>();

            int frequency;
            char c;
            TreeNode charNode;
            foreach (KeyValuePair<char, int> item in charMap)
            {
                frequency = item.Value;
                c = item.Key;
                charNode = new TreeNode(frequency, c, null, null);
                heap.Enqueue(charNode, frequency);
            }
            return heap;
        }

        private static TreeNode? BuildHuffmanTree(SimplePriorityQueue<TreeNode> heap)
        {
            if (heap.Count == 0)
            {
                return null;
            }
            while (heap.Count > 1)
            {
                TreeNode right = heap.Dequeue();
                TreeNode left = heap.Dequeue();
                int parentFrequency = left.Frequency + right.Frequency;

                TreeNode parent = new TreeNode(parentFrequency, '\0', left, right);
                heap.Enqueue(parent, parentFrequency);
            }
            if (heap.Count == 1)
            {
                return heap.Dequeue();
            }
            return null;
        }

        private static Dictionary<char, BitArray> BuildHuffmanCode(TreeNode? node)
        {
            Dictionary<char, BitArray> huffmanCode = new Dictionary<char, BitArray>();
            Queue<TreeNode> queue = new Queue<TreeNode>();
            if (!(node == null))
            {
                queue.Enqueue(node);
            }

            while (queue.Count > 0)
            {
                TreeNode item = queue.Dequeue();

                if (item.Left != null)
                {
                    item.Left.HFCode = item.GetLShiftPlusOne();
                    queue.Enqueue(item.Left);
                }
                if (item.Right != null)
                {
                    item.Right.HFCode = item.GetLShiftPlusZero();
                    queue.Enqueue(item.Right);
                }
                if (item.IsLeafNode())
                {
                    huffmanCode.Add(item.Character, item.HFCode);
                }
            }
            return huffmanCode;
        }

        private static BitArray EncodeStringToBits(string str, Dictionary<char, BitArray> huffmanCode)
        {
            BitArray bitArray = new BitArray(0);
            foreach (char c in str)
            {
                if (huffmanCode.ContainsKey(c))
                {
                    //Make deep copy of item value
                    BitArray bitsToAdd = new BitArray(huffmanCode[c]);
                    
                    int extend = bitsToAdd.Count;

                    bitArray.Length += extend;
                    bitArray.LeftShift(extend);

                    int currentLen = bitArray.Count;
                    bitsToAdd.Length += (currentLen - extend);
                    
                    bitArray.Or(bitsToAdd);
                }
                else
                {
                    throw new KeyNotFoundException(String.Format("{0} does not exist.", c.ToString()));
                }
            }
            return bitArray;
        }

        private static StringBuilder DecodeBitsToString(BitArray bitArray, TreeNode? huffmanCode)
        {
            if (huffmanCode == null)
            {
                throw new Exception("node is null");
            }
            StringBuilder stringBuilder = new StringBuilder();
            TreeNode node = huffmanCode;

            for (int i = bitArray.Count - 1; i >= 0; i--)
            {
                bool bit = bitArray.Get(i);

                if (node == null)
                {
                    throw new Exception("node is null");
                }

                if (!(node == null) && node.IsLeafNode())
                {
                    stringBuilder.Append(node.Character);
                    node = huffmanCode;
                }

                if (bit.Equals(false))
                {
                    node = node.Right;
                }
                else  //c is '1'
                {
                    node = node.Left;
                }

                if (!(node == null) && node.IsLeafNode())
                {
                    stringBuilder.Append(node.Character);
                    node = huffmanCode;
                }
            }
            return stringBuilder;
        }
    }
}
