using System;
using System.Collections;
using System.Collections.Generic;

namespace HuffmanCode
{
    public class TreeNode
    {
        public const int HFCodeStartSize = 0;
        public char Character { get; set; }
        public int Frequency { get; set; }
        public TreeNode? Left { get; set; }
        public TreeNode? Right { get; set; }
        public BitArray HFCode { get; set; }

        public TreeNode(int frequency, char character = '\0', TreeNode? left = null, TreeNode? right = null)
        {
            this.Frequency = frequency;
            this.Character = character;
            this.Left = left;
            this.Right = right;
            this.HFCode = new BitArray(HFCodeStartSize);
        }

        public bool IsLeafNode()
        {
            return (Left == null) && (Right == null);
        }

        public override string ToString()
        {
            return String.Format("( Freq={0}, Char={1} )", Frequency, Character);
        }

        public BitArray GetLShiftPlusZero()
        {
            BitArray copy = new BitArray(HFCode);
            ++copy.Length;
            copy.LeftShift(1);
            return copy;
        }

        public BitArray GetLShiftPlusOne()
        {
            BitArray copy = new BitArray(HFCode);
            ++copy.Length;
            copy.LeftShift(1);
            copy.Set(0, true);
            return copy;
        }

        public void StripLeftZeroes()
        {
            int length = HFCode.Count;
            BitArray flag = new BitArray(length);
            flag.Set(length - 1, true);

            while (HFCode.Length > 1 && !(flag.And(HFCode).Get(length - 1)))
            {
                HFCode.Length--;
                length--;
                flag.RightShift(1);
                flag.Length--;
                flag.Set(length - 1, true);
            }
        }

        public void PrintSelfBFS()
        {
            PrintTreeBFS(this);
        }

        public static void PrintTreeBFS(TreeNode? node)
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
    }
}
