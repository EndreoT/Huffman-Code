using System.Text;

namespace HuffmanCode.Extensions
{
    public static class HuffmanTreeNodeExtensions
    {
        public static void PrintBFS(this HuffmanTreeNode treeNode)
        {
            Console.WriteLine(GetTreeRepresentation(treeNode));
            Console.WriteLine();
        }

        private static string GetTreeRepresentation(HuffmanTreeNode? node)
        {
            if (node is null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            StringBuilder sb = new();
            Queue<HuffmanTreeNode> queue = new();
            queue.Enqueue(node);

            int levelCount = 1;
            while (queue.Count > 0)
            {
                HuffmanTreeNode item = queue.Dequeue();
                levelCount--;

                if (item.Left != null)
                {
                    queue.Enqueue(item.Left);
                }
                if (item.Right != null)
                {
                    queue.Enqueue(item.Right);
                }
                sb.Append(item);

                if (levelCount == 0)
                {
                    sb.Append('\n');
                    levelCount = queue.Count;
                }
            }

            return sb.ToString();
        }
    }
}