using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using HuffmanCode.ExtensionMethods;

using Priority_Queue;

namespace HuffmanCode
{
    class HuffmanEncode
    {
        public static Tuple<BitArray, TreeNode> BuildHuffmanCode(string str)
        {
            Dictionary<char, int> charCount = BuildFrequencyMap(str);

            SimplePriorityQueue<TreeNode> heap = BuildPriorityQueue(charCount);

            TreeNode? tree = BuildTree(heap);

            //foreach (KeyValuePair<char, int> item in charCount)
            //{
            //    Console.WriteLine(String.Format("{0} {1}", item.Key.ToString(), item.Value.ToString()));
            //}


            //PrintTreeBFS(tree);

            Dictionary<char, BitArray> huffmanCode = HuffmanCode(tree);

            foreach (KeyValuePair<char, BitArray> item in huffmanCode)
            {
                string bitStr = BitArrayFullString(item.Value);
                Console.WriteLine(String.Format("{0}, NumBits={1}, {2}", item.Key.ToString(), item.Value.Count, bitStr));
            }

            BitArray concatenatedBits = ConcatenateBits(str, huffmanCode);

            Console.WriteLine(BitArrayFullString(concatenatedBits));

            return new Tuple<BitArray, TreeNode>(concatenatedBits, tree);
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

        public static string BitArrayString(BitArray bitArray)
        { 
            Stack<char> charStack = new Stack<char>();
            int index = 0;
            bool bit;
            char bitChar;
            while (index < bitArray.Count)
            {
                bit = bitArray.Get(index);
                bitChar = bit ? '1' : '0';
                charStack.Push(bitChar);
                ++index;
            }

            while (charStack.Count > 1 && charStack.Peek() == '0')
            {
                charStack.Pop();
            }
 

            StringBuilder stringBuilder = new StringBuilder(8);
            foreach (char charBit in charStack)
            {
                stringBuilder.Append(charBit);
            }
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

        private static TreeNode? BuildTree(SimplePriorityQueue<TreeNode> heap)
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

        private static Dictionary<char, BitArray> HuffmanCode(TreeNode? node)
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
                    item.StripLeftZeroes();
                    huffmanCode.Add(item.Character, item.HFCode);
                }
            }
            return huffmanCode;
        }

        private static BitArray ConcatenateBits(string str, Dictionary<char, BitArray> huffmanCode)
        {
            BitArray bitArray = new BitArray(0);
            foreach (char c in str)
            {
                if (huffmanCode.ContainsKey(c))
                {
                    //Make deep copy of item value
                    BitArray bits = new BitArray(huffmanCode[c]);
                    
                    int extend = bits.Count;

                    bitArray.Length += extend;
                    bitArray.LeftShift(extend);

                    int current = bitArray.Count;
                    bits.Length += (current - extend);
                    
                    bitArray.Or(bits);
                }
                else
                {
                    throw new KeyNotFoundException(String.Format("{0} does not exist.", c.ToString()));
                }
            }
            return bitArray;
        }
    }
}
