using HuffmanCode.Extensions;
using System.Collections;
using System.Text;

namespace HuffmanCode
{
    internal class HuffmanTreeNode
    {
        public const int HuffmanCodeStartSize = 0;

        public Rune Character { get; }

        public int Frequency { get; }

        public HuffmanTreeNode? Left { get; set; }

        public HuffmanTreeNode? Right { get; set; }

        private BitArray? _huffmanCode = null;

        public BitArray HuffmanCode
        {
            private get { return _huffmanCode is null ? new BitArray(HuffmanCodeStartSize) : _huffmanCode; }
            set { _huffmanCode = value; }
        }

        public HuffmanTreeNode(int frequency, Rune character, HuffmanTreeNode? left = null, HuffmanTreeNode? right = null)
        {
            Frequency = frequency;
            Character = character;
            Left = left;
            Right = right;
        }

        public BitArray GetHuffmanCodeCopy()
        {
            return new BitArray(HuffmanCode);
        }

        public bool IsLeafNode()
        {
            return Left is null && Right is null;
        }

        public bool IsPseudoEOFCharacter()
        {
            return Character.Value == Constants.PseudoEndOfFileChar;
        }

        public override string ToString()
        {
            return $"(Freq={Frequency}, Char={Character})";
        }

        public Dictionary<Rune, BitArray> BuildCharacterMapToHuffmanCode()
        {
            Dictionary<Rune, BitArray> huffmanCode = new();
            Queue<HuffmanTreeNode> queue = new();

            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                HuffmanTreeNode node = queue.Dequeue();
                if (node.IsLeafNode())
                {
                    huffmanCode.Add(node.Character, node.HuffmanCode);
                    continue;
                }

                HuffmanTreeNode? left = node.Left;
                if (left is not null)
                {
                    left.HuffmanCode = node.GetHuffmanCodeCopy().LeftShiftOncePlusOne();
                    queue.Enqueue(left);
                }

                HuffmanTreeNode? right = node.Right;
                if (right is not null)
                {
                    right.HuffmanCode = node.GetHuffmanCodeCopy().LeftShiftOnce();
                    queue.Enqueue(right);
                }

            }
            return huffmanCode;
        }
    }
}