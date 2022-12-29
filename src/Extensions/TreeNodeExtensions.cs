using System.Text;

namespace HuffmanCode.Extensions
{
    public static class TreeNodeExtensions
    {
        public static void PrintBFS(this TreeNode treeNode)
        {
            Console.WriteLine(GetTreeRepresentation(treeNode));
            Console.WriteLine();
        }

        private static string GetTreeRepresentation(TreeNode? node)
        {
            if (node is null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            StringBuilder sb = new();
            Queue<TreeNode> queue = new();
            queue.Enqueue(node);

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