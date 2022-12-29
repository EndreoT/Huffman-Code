using HuffmanCode.Extensions;

namespace HuffmanCode;

internal class Program
{
    private static async Task Main()
    {
        string str = "AEDCABDECBADAECADBAABEAADCBACEABDBAAACA";

        Console.WriteLine($"{str}: original string");

        await using EncodedStringContext context = HuffmanCode.EncodeString(str);

        context.HuffmanTreeRootNode.PrintBFS();

        string decodedString = HuffmanCode.DecodeToString(context.Stream, context.NumBits, context.HuffmanTreeRootNode);

        Console.WriteLine($"{str}: original string");
        Console.WriteLine($"{decodedString}: decoded string");

        Console.WriteLine();
        bool matches = str.Equals(decodedString.ToString());
        Console.WriteLine($"Origin string matches encoded then decoded result: {matches}");
    }
}