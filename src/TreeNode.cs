using System.Collections;

namespace HuffmanCode
{
    public class TreeNode
    {
        public const int HuffmanCodeStartSize = 0;

        public char Character { get; }

        public int Frequency { get; }

        public TreeNode? Left { get; set; }

        public TreeNode? Right { get; set; }

        public BitArray HuffmanCode { private get; set; }

        public TreeNode(int frequency, char character = char.MinValue, TreeNode? left = null, TreeNode? right = null)
        {
            Frequency = frequency;
            Character = character;
            Left = left;
            Right = right;
            HuffmanCode = new BitArray(HuffmanCodeStartSize);
        }

        public BitArray GetHuffmanCodeCopy()
        {
            return new BitArray(HuffmanCode);
        }

        public bool IsLeafNode()
        {
            return Left is null && Right is null;
        }

        public override string ToString()
        {
            return $"(Freq={Frequency}, Char={Character})";
        }
    }
}