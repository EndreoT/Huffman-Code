using System.Text;

namespace HuffmanCode;

internal class HuffmanTreeBuilder
{
    public static HuffmanTreeNode BuildHuffmanTree(Dictionary<Rune, uint> charFrequency)
    {
        PriorityQueue<HuffmanTreeNode, uint> heap = BuildTreeNodeFrequencyHeap(charFrequency);

        return BuildHuffmanTreeInternal(heap);
    }

    public static PriorityQueue<HuffmanTreeNode, uint> BuildTreeNodeFrequencyHeap(IDictionary<Rune, uint> charFrequency)
    {
        PriorityQueue<HuffmanTreeNode, uint> heap = new();

        foreach (KeyValuePair<Rune, uint> item in charFrequency)
        {
            uint frequency = item.Value;
            Rune c = item.Key;
            HuffmanTreeNode charNode = new(frequency, c, null, null);
            heap.Enqueue(charNode, frequency);
        }
        return heap;
    }

    private static HuffmanTreeNode BuildHuffmanTreeInternal(PriorityQueue<HuffmanTreeNode, uint> heap)
    {
        while (heap.Count > 1)
        {
            HuffmanTreeNode right = heap.Dequeue();
            HuffmanTreeNode left = heap.Dequeue();
            uint parentFrequency = left.Frequency + right.Frequency;

            HuffmanTreeNode parent = new(parentFrequency, new Rune(char.MinValue), left, right);
            heap.Enqueue(parent, parentFrequency);
        }

        return heap.Dequeue();
    }
}