using System.Collections;

namespace HuffmanCode;

public interface IHuffmanCode
{
    string DecodeToString(Stream stream);

    Stream EncodeString(string str);

    void PrintHuffmanCode(Dictionary<char, BitArray> huffmanCode);
}