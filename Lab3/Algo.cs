using Utils;

namespace Lab3;

public class Algo
{
    private static uint CreateBitmapForMovingBit(byte[] src, int bitInitialPosition, int bitDestination)
    {
        return (uint) (((src[bitInitialPosition / 8] >> (7 - bitInitialPosition % 8)) & 0x01) << bitDestination);
    }

    private static byte CreateBitmapForMovingBit(uint src, int bitInitialPosition, int bitDestination)
    {
        return (byte) (((src >> (31 - bitInitialPosition)) & 0x00000001) << bitDestination);
    }

    private static uint CreateBitmapForMovingBitUint(uint src, int bitInitialPosition, int bitDestination)
    {
        return ((src << (bitInitialPosition)) & 0x80000000) >> bitDestination;
    }

    private static uint CalculateSBoxBit(byte src)
    {
        return (uint) ((src & 0x20) | ((src & 0x1f) >> 1) | ((src & 0x01) << 4));
    }

    private static byte[][] CreateKeys(byte[] key, bool encrypt)
    {
        var result = Enumerable.Repeat(1, 16).Select(_ => new byte[6]).ToArray();
        uint C = 0, D = 0;
        int i, j;

        for (i = 0, j = 31; i < 28; ++i, --j)
            C |= CreateBitmapForMovingBit(key, (int) Constants.FirstKeyPermutationTableFirstPart[i], j);

        for (i = 0, j = 31; i < 28; ++i, --j)
            D |= CreateBitmapForMovingBit(key, (int) Constants.FirstKeyPermutationTableSecondPart[i], j);

        int toGen;
        for (i = 0; i < 16; ++i)
        {
            C = ((C << (int) Constants.ShiftBits[i]) | (C >> (28 - (int) Constants.ShiftBits[i]))) & 0xfffffff0;
            D = ((D << (int) Constants.ShiftBits[i]) | (D >> (28 - (int) Constants.ShiftBits[i]))) & 0xfffffff0;

            if (encrypt)
                toGen = i;
            else
                toGen = 15 - i;

            for (j = 0; j < 6; ++j)
                result[toGen][j] = 0;

            for (j = 0; j < 24; ++j)
                result[toGen][j / 8] |=
                    CreateBitmapForMovingBit(C, (int) Constants.SecondKeyPermutationTable[j], 7 - j % 8);

            for (; j < 48; ++j)
                result[toGen][j / 8] |=
                    CreateBitmapForMovingBit(D, (int) Constants.SecondKeyPermutationTable[j] - 28, 7 - j % 8);
        }

        return result;
    }

    private static uint Permutate(byte[] src, IEnumerable<int> sequence, bool inverse = false)
    {
        return sequence
            .Select((value, idx) => CreateBitmapForMovingBit(src, value, inverse ? 31 - idx : idx))
            .Aggregate((uint) 0, (current, next) => current | next);
    }

    private static uint Permutate(uint src, IEnumerable<int> sequence, bool inverse = false)
    {
        return sequence
            .Select((value, idx) => CreateBitmapForMovingBitUint(src, value, inverse ? 31 - idx : idx))
            .Aggregate((uint) 0, (current, next) => current | next);
    }

    private static uint Round(uint t, byte[] key)
    {
        var expanded1 = Permutate(t, Constants.ExpansionTable.Take(24));
        var expanded2 = Permutate(t, Constants.ExpansionTable.Skip(24));

        var xored = new byte[6];

        var bytes1 = BitConverter.GetBytes(expanded1).ToArray();
        xored[0] = (byte) (bytes1[^1] ^ key[0]);
        xored[1] = (byte) (bytes1[^2] ^ key[1]);
        xored[2] = (byte) (bytes1[^3] ^ key[2]);
        var bytes2 = BitConverter.GetBytes(expanded2).ToArray();
        xored[3] = (byte) (bytes2[^1] ^ key[3]);
        xored[4] = (byte) (bytes2[^2] ^ key[4]);
        xored[5] = (byte) (bytes2[^3] ^ key[5]);

        uint result = (uint) ((Constants.SBox[0][CalculateSBoxBit((byte) (xored[0] >> 2))] << 28) |
                              (Constants.SBox[1][CalculateSBoxBit((byte) (((xored[0] & 0x03) << 4) | (xored[1] >> 4)))] << 24) |
                              (Constants.SBox[2][CalculateSBoxBit((byte) (((xored[1] & 0x0f) << 2) | (xored[2] >> 6)))] << 20) |
                              (Constants.SBox[3][CalculateSBoxBit((byte) (xored[2] & 0x3f))] << 16) |
                              (Constants.SBox[4][CalculateSBoxBit((byte) (xored[3] >> 2))] << 12) |
                              (Constants.SBox[5][CalculateSBoxBit((byte) (((xored[3] & 0x03) << 4) | (xored[4] >> 4)))] << 8) |
                              (Constants.SBox[6][CalculateSBoxBit((byte) (((xored[4] & 0x0f) << 2) | (xored[5] >> 6)))] << 4) |
                              Constants.SBox[7][CalculateSBoxBit((byte) (xored[5] & 0x3f))]);

        return Permutate(result, Constants.StraightPermutationTable);
    }

    private static byte[] InversePermutation(uint left, uint right, int[] sequence)
    {
        byte[] result = {0, 0, 0, 0, 0, 0, 0, 0};

        for (var i = 0; i < sequence.Length; i++)
        {
            if (sequence[i] >= 32)
            {
                result[i / 8] |= CreateBitmapForMovingBit(right, sequence[i] - 32, 7 - (i % 8));
            }
            else
            {
                result[i / 8] |= CreateBitmapForMovingBit(left, sequence[i], 7 - (i % 8));
            }
        }
        
        return result;
    }

    public static byte[] Crypt(byte[] input, byte[] key, bool encrypt)
    {
        var keys = CreateKeys(key, encrypt);

        Console.WriteLine("Keys");
        foreach (var k in keys)
        {
            Console.WriteLine(PrintHelper.ToFancyByteArray(key));
        }
        Console.WriteLine();

        var t1 = Permutate(input, Constants.InitialPermutationTable.Take(32), true);

        var t2 = Permutate(input, Constants.InitialPermutationTable.Skip(32), true);

        Console.WriteLine("Rounds");
        for (var i = 0; i < 15; i++)
        {
            var temp = t2;
            t2 = Round(t2, keys[i]) ^ t1;
            t1 = temp;

            Console.WriteLine($"{PrintHelper.ToFancyByteArray(t1)}, {PrintHelper.ToFancyByteArray(t2)}");
        }

        t1 = Round(t2, keys[15]) ^ t1;
        Console.WriteLine(PrintHelper.ToFancyByteArray(t1));

        return InversePermutation(t1, t2, Constants.ReverseInitialPermutationTable);
    }

    
}