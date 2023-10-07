namespace HuffmanCode.Unittests
{
    [TestClass]
    public class HuffmanCodeTests
    {
        private const string FilePath = ".\\test";

        [TestMethod]
        public async Task TestFullEncodeDecode()
        {
            string str = "👩🏽‍🚒‍‍🚒🚒 \udc69 \ud83d\udc69\ud83c\udffd \ud83d\udc02  👩🏽‍🚒A👩🏽‍🚒ED1CABDEC🐂BADpAE11CAD🐂B AABEAAD 🐂 p CBACppqEABD2BAAACA ⌀ X";
            //string str = "AED1CABDECBADpAE11CADB AABEAAD p CBACppqEABD2BAAACA X";
            //string str = "";

            Console.WriteLine(@$"original string: ""{str}""");

            await EncodeStringToFileAsync(str);

            string decodedString = DecodeStringFromFile();

            Console.WriteLine(@$"original string: ""{str}""");
            Console.WriteLine($"{decodedString}: decoded string");

            Console.WriteLine();
            bool matches = str.Equals(decodedString);
            Console.WriteLine($"Original string matches encoded the decoded result: {matches}");
        }

        public static async Task EncodeStringToFileAsync(string str)
        {
            await using Stream stream = HuffmanCode.EncodeString(str.AsSpan());

            using FileStream fileStream = File.Create(FilePath);

            stream.Seek(0, SeekOrigin.Begin);
            await stream.CopyToAsync(fileStream);
        }

        private static string DecodeStringFromFile()
        {
            using FileStream fileStream = File.OpenRead(FilePath);
            return HuffmanCode.DecodeToString(fileStream);
        }
    }
}