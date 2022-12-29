namespace HuffmanCode;

public class HuffmanTreeBuilder
{
    public static TreeNode BuildHuffmanTree(IDictionary<char, int> charCount)
    {
        PriorityQueue<TreeNode, int> heap = BuildTreeNodeFrequencyHeap(charCount);

        return BuildHuffmanTreeInternal(heap);
    }

    private static PriorityQueue<TreeNode, int> BuildTreeNodeFrequencyHeap(IDictionary<char, int> charMap)
    {
        PriorityQueue<TreeNode, int> heap = new();

        foreach (KeyValuePair<char, int> item in charMap)
        {
            int frequency = item.Value;
            char c = item.Key;
            TreeNode charNode = new(frequency, c, null, null);
            heap.Enqueue(charNode, frequency);
        }
        return heap;
    }

    private static TreeNode BuildHuffmanTreeInternal(PriorityQueue<TreeNode, int> heap)
    {
        while (heap.Count > 1)
        {
            TreeNode right = heap.Dequeue();
            TreeNode left = heap.Dequeue();
            int parentFrequency = left.Frequency + right.Frequency;

            TreeNode parent = new(parentFrequency, char.MinValue, left, right);
            heap.Enqueue(parent, parentFrequency);
        }

        return heap.Dequeue();
    }
}