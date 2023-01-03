namespace HuffmanCode;

internal class Program
{
    private const string FilePath = ".\\test";

    private static async Task Main()
    {
        string str = "AEDCABDECBADpAECADB AABEAAD p CBACppqEABDBAAACAX";

        Console.WriteLine($"{str}: original string");

        await EncodeStringToFileAsync(str);

        string decodedString = DecodeStringFromFile();

        Console.WriteLine($"{str}: original string");
        Console.WriteLine($"{decodedString}: decoded string");

        Console.WriteLine();
        bool matches = str.Equals(decodedString.ToString());
        Console.WriteLine($"Original string matches encoded the decoded result: {matches}");
    }

    public static async Task EncodeStringToFileAsync(string str)
    {
        IHuffmanCode huffmanCode = new HuffmanCode();

        await using Stream stream = huffmanCode.EncodeString(str);

        using var fileStream = File.Create(FilePath);

        stream.Seek(0, SeekOrigin.Begin);
        stream.CopyTo(fileStream);
    }

    private static string DecodeStringFromFile()
    {
        IHuffmanCode huffmanCode = new HuffmanCode();
        using FileStream fileStream = File.OpenRead(FilePath);
        return huffmanCode.DecodeToString(fileStream);
    }
}