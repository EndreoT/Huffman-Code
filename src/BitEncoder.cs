using System.Collections;
using System.Text;

namespace HuffmanCode;

internal static class BitEncoder
{
    public static BitArray EncodeInputToBits(ReadOnlySpan<char> input, Dictionary<Rune, BitArray> huffmanCode)
    {
        BitArray bitArray = new(0);

        foreach (Rune c in input.EnumerateRunes())
        {
            bitArray = AddBits(bitArray, huffmanCode, c);
        }

        AddBits(bitArray, huffmanCode, new Rune(Constants.PseudoEndOfFileChar));

        // Add padding to reach a full byte if needed
        int numBitsInLastByte = bitArray.Length % 8;
        if (numBitsInLastByte > 0)
        {
            int shiftLen = 8 - numBitsInLastByte;
            bitArray.Length += shiftLen;
            bitArray.LeftShift(shiftLen);
        }

        return bitArray;
    }

    private static BitArray AddBits(BitArray bitArray, Dictionary<Rune, BitArray> huffmanCode, Rune c)
    {
        if (!huffmanCode.TryGetValue(c, out BitArray? bitsToAdd))
        {
            throw new KeyNotFoundException($"Character '{c}' does not exist in huffman code.");
        }

        int numBitsToAdd = bitsToAdd.Count;

        bitArray.Length += numBitsToAdd;
        bitArray.LeftShift(numBitsToAdd);

        for (int i = numBitsToAdd - 1; i >= 0; i--)
        {
            bitArray[i] = bitsToAdd[i];
        }

        return bitArray;
    }
}