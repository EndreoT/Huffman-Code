using System.Collections;

namespace HuffmanCode
{
    public class HuffmanTreeNode
    {
        public const int HuffmanCodeStartSize = 0;

        public char Character { get; }

        public int Frequency { get; }

        public HuffmanTreeNode? Left { get; set; }

        public HuffmanTreeNode? Right { get; set; }

        public BitArray HuffmanCode { private get; set; }

        public HuffmanTreeNode(int frequency, char character = char.MinValue, HuffmanTreeNode? left = null, HuffmanTreeNode? right = null)
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

        public bool IsPseudoEOFCharacter()
        {
            return Character == Constants.PseudoEndOfFileChar;
        }

        public override string ToString()
        {
            return $"(Freq={Frequency}, Char={Character})";
        }
    }
}