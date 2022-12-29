namespace HuffmanCode;

public sealed class EncodedStringContext : IAsyncDisposable
{
    public Stream Stream { get; }

    public int NumBits { get; }

    public TreeNode HuffmanTreeRootNode { get; }

    public EncodedStringContext(Stream stream, int numBits, TreeNode rootNode)
    {
        Stream = stream;
        NumBits = numBits;
        HuffmanTreeRootNode = rootNode;
    }

    public async ValueTask DisposeAsync()
    {
        if (Stream is not null)
        {
            await Stream.DisposeAsync();
        }
    }
}