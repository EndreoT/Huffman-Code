using System.Text;

namespace HuffmanCode;

internal class HuffmanTreeBuilder
{
    public static HuffmanTreeNode BuildHuffmanTree(Dictionary<Rune, int> charFrequency)
    {
        PriorityQueue<HuffmanTreeNode, int> heap = BuildTreeNodeFrequencyHeap(charFrequency);

        return BuildHuffmanTreeInternal(heap);
    }

    public static PriorityQueue<HuffmanTreeNode, int> BuildTreeNodeFrequencyHeap(IDictionary<Rune, int> charFrequency)
    {
        PriorityQueue<HuffmanTreeNode, int> heap = new();

        foreach (KeyValuePair<Rune, int> item in charFrequency)
        {
            int frequency = item.Value;
            Rune c = item.Key;
            HuffmanTreeNode charNode = new(frequency, c, null, null);
            heap.Enqueue(charNode, frequency);
        }
        return heap;
    }

    private static HuffmanTreeNode BuildHuffmanTreeInternal(PriorityQueue<HuffmanTreeNode, int> heap)
    {
        while (heap.Count > 1)
        {
            HuffmanTreeNode right = heap.Dequeue();
            HuffmanTreeNode left = heap.Dequeue();
            int parentFrequency = left.Frequency + right.Frequency;

            HuffmanTreeNode parent = new(parentFrequency, new Rune(char.MinValue), left, right);
            heap.Enqueue(parent, parentFrequency);
        }

        return heap.Dequeue();
    }
}