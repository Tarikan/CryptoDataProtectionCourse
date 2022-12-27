using System.Numerics;
using System;

namespace Utils;
public static class RandomNumberHelper
{
    private static readonly Random _random = new();

    public static BigInteger RandomIntegerInRange(BigInteger min, BigInteger max)
    {
        var initialMin = min;

        (min, max) = (0, max - min);
        byte[] bytes = max.ToByteArray();
        BigInteger R;

        do
        {
            _random.NextBytes(bytes);
            bytes[bytes.Length - 1] &= (byte)0x7F; //force sign bit to positive
            R = new BigInteger(bytes);
        } while (R >= max);

        return R + initialMin;
    }
}
