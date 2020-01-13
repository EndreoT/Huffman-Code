using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Priority_Queue;


namespace HuffmanCode
{
    class Program
    {
        static void Main(string[] args)
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
  
            string cwd = Directory.GetCurrentDirectory();
            const string fileName = "binaryFile.dat";
            string filePath = Path.Combine(cwd, fileName);
            Console.WriteLine(Directory.GetParent(Directory.GetParent(filePath).ToString()));

            var (encodedString, huffmanCodeTree) = HuffmanCode.EncodeString(charArray);

            string decodedString = HuffmanCode.DecodeString(encodedString, huffmanCodeTree);

            Console.WriteLine(charArray);
            Console.WriteLine(decodedString);

            Console.WriteLine();
            bool matches = charArray.Equals(decodedString.ToString());
            Console.WriteLine(String.Format("Origin string matches encoded then decoded result: {0}", matches));
        }
    }
}
