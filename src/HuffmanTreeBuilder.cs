namespace HuffmanCode;

internal class HuffmanTreeBuilder
{
    public static HuffmanTreeNode BuildHuffmanTree(Dictionary<char, int> charCount)
    {
        PriorityQueue<HuffmanTreeNode, int> heap = BuildTreeNodeFrequencyHeap(charCount);

        return BuildHuffmanTreeInternal(heap);
    }

    public static PriorityQueue<HuffmanTreeNode, int> BuildTreeNodeFrequencyHeap(IDictionary<char, int> charMap)
    {
        PriorityQueue<HuffmanTreeNode, int> heap = new();

        foreach (KeyValuePair<char, int> item in charMap)
        {
            int frequency = item.Value;
            char c = item.Key;
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

            HuffmanTreeNode parent = new(parentFrequency, char.MinValue, left, right);
            heap.Enqueue(parent, parentFrequency);
        }

        return heap.Dequeue();
    }
}