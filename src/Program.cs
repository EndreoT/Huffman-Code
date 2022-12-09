namespace Huffman_Code;

internal class Program
{
    private static void Main(string[] args)
    {
        string charArray;
        if (args.Length > 0)
        {
            charArray = args[0];
        }
        else
        {
            charArray = "AEDCABDECBADAECADBAABEAADCBACEABDBAAACA";
        }

        //string cwd = Directory.GetCurrentDirectory();
        //const string fileName = "binaryFile.dat";
        //string filePath = Path.Combine(cwd, fileName);
        //Console.WriteLine(Directory.GetParent(Directory.GetParent(filePath).ToString()));

        var (encodedString, huffmanCodeTree) = HuffmanCode.EncodeString(charArray);

        string decodedString = HuffmanCode.DecodeString(encodedString, huffmanCodeTree);

        Console.WriteLine(charArray);
        Console.WriteLine(decodedString);

        Console.WriteLine();
        bool matches = charArray.Equals(decodedString.ToString());
        Console.WriteLine($"Origin string matches encoded then decoded result: {matches}");
    }
}