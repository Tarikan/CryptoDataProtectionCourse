namespace lab9;
public static class MD5
{
    public static string Calculate(byte[] input)
    {
        var a0 = Constants.A;
        var b0 = Constants.B;
        var c0 = Constants.C;
        var d0 = Constants.D;

        var padding = ((56 - ((input.Length + 1) % 64)) % 64) + 9;

        var paddedInput = new byte[input.Length + padding];
        Array.Copy(input, paddedInput, input.Length);
        paddedInput[input.Length] = 0x80;

        var lengthAsByteArray = BitConverter.GetBytes(input.Length * 8);
        Array.Copy(lengthAsByteArray, 0, paddedInput, paddedInput.Length - 8, 4);

        for (var i = 0; i < paddedInput.Length / 64; ++i)
        {
            var M = new uint[16];
            for (int j = 0; j < 16; ++j)
            {
                M[j] = BitConverter.ToUInt32(paddedInput, (i * 64) + (j * 4));
            }

            var A = a0;
            var B = b0;
            var C = c0;
            var D = d0;
            uint F = 0;
            uint g = 0;

            for (uint k = 0; k < 64; ++k)
            {
                (A, B, C, D, F, g) = Permutate(M, A, B, C, D, F, g, k);
            }

            a0 += A;
            b0 += B;
            c0 += C;
            d0 += D;
        }

        return GetByteString(a0)
            + GetByteString(b0)
            + GetByteString(c0)
            + GetByteString(d0);
    }

    private static (uint A, uint B, uint C, uint D, uint F, uint g)
        Permutate(uint[] M, uint A, uint B, uint C, uint D, uint F, uint g, uint k)
    {
        if (k <= 15)
        {
            F = (B & C) | (~B & D);
            g = k;
        }
        else if (k is >= 16 and <= 31)
        {
            F = (D & B) | (~D & C);
            g = (5 * k + 1) % 16;
        }
        else if (k is >= 32 and <= 47)
        {
            F = B ^ C ^ D;
            g = (3 * k + 5) % 16;
        }
        else if (k >= 48)
        {
            F = C ^ (B | ~D);
            g = 7 * k % 16;
        }

        (A, B, C, D) =
            (D, B + LeftRotate((A + F + Constants.K[k] + M[g]), Constants.RoundShiftValues[k]), B, C);

        return (A, B, C, D, F, g);
    }

    private static string GetByteString(uint x) =>
        string.Concat(BitConverter.GetBytes(x)
            .Select(y => y.ToString("x2")));

    private static uint LeftRotate(uint x, int c)
    {
        return (x << c) | (x >> (32 - c));
    }
}
