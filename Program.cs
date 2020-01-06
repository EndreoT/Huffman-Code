using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Priority_Queue;


namespace HuffmanCode
{
    class Program
    {
        static private void PrintTreeBFS(TreeNode? node)
        {
            Queue<TreeNode> queue = new Queue<TreeNode>();
            if (!(node == null))
            {
                queue.Enqueue(node);
            }

            int levelCount = 1;
            while (queue.Count > 0)
            {
                TreeNode item = queue.Dequeue();
                levelCount--;
                
                if (item.Left != null)
                {
                    queue.Enqueue(item.Left);
                }
                if (item.Right != null)
                {
                    queue.Enqueue(item.Right);
                }
                Console.Write(item);
                if (levelCount == 0)
                {
                    Console.WriteLine();
                    levelCount = queue.Count;
                }
            }
        }


        private static StringBuilder DecodeBitString(BitArray bitArray, TreeNode huffmanCode)
        {
            StringBuilder stringBuilder = new StringBuilder();
            TreeNode node = huffmanCode;

            for (int i = bitArray.Count - 1; i >= 0; i--)
            {
                bool bit = bitArray.Get(i);

                if (node == null)
                {
                    throw new Exception("node is null");
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

        static void Main(string[] args)
        {
            const string charArray = "AEDCABDECBADAECADBAABEAADCBACEABDBAAACA";


            Tuple<BitArray, TreeNode> res = HuffmanEncode.BuildHuffmanCode(charArray);

            StringBuilder decodedString = DecodeBitString(res.Item1, res.Item2);

            Console.WriteLine(charArray);
            Console.WriteLine(decodedString);

            Console.WriteLine(charArray.Equals(decodedString.ToString()));






            //BitArray copy = new BitArray(32);
            //Console.WriteLine(HuffmanEncode.BitArrayFullString(copy));
            //copy.Set(0, true);
            ////copy.Set(2, true);
            //Console.WriteLine(HuffmanEncode.BitArrayFullString(copy));
            //int length = copy.Count;
            //BitArray flag = new BitArray(length);
            //flag.Set(length - 1, true);
            //while (copy.Length > 1 && !(flag.And(copy).Get(length - 1)))
            //{
            //    copy.Length--;
            //    length--;
            //    flag.RightShift(1);
            //    flag.Length--;
            //    flag.Set(length - 1, true);
            //}

            //Console.WriteLine(HuffmanEncode.BitArrayFullString(copy));








        }
    }
}
