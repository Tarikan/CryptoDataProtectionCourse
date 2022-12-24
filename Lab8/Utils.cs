using Lab5;
using System.Numerics;
using System.Security.Cryptography;

namespace Lab8;
public static class Utils
{
    private static readonly RandomNumberGenerator rnd = RandomNumberGenerator.Create();

    public static BigInteger GenerateRandomBigInt(BigInteger? minValue = null, BigInteger? maxValue = null, int byteLen = 8)
    {
        if (byteLen <= 0)
        {
            throw new ArgumentException(nameof(byteLen));
        }

        byte[] bytes = new byte[byteLen];
        rnd.GetBytes(bytes);
        bytes[bytes.Length - 1] &= (byte)0x7F; //force sign bit to positive

        var result = new BigInteger(bytes);

        if (minValue.HasValue && result < minValue.Value)
        {
            result += minValue.Value;
        }

        if (maxValue.HasValue && result > maxValue.Value)
        {
            result = result % maxValue.Value;
        }

        return result;
    }

    public static BigInteger GenerateRandomPrime()
    {
        var random = new Random();
        var randomNum = random.Next();
        while (true)
        {
            if (RabinMiller.IsPrime(randomNum, 1))
            {
                return randomNum;
            }
            randomNum++;
        }
    }

    public static bool IsPrimitiveRoot(BigInteger p, BigInteger g)
    {
        for (var i = g; i < p; i++)
        {
            if (BigInteger.ModPow(g, i, p) == 1)
            {
                return false;
            }
        }

        return true;
    }

    public static BigInteger GetG(BigInteger p)
    {
        while (true)
        {
            var g = GenerateRandomBigInt(2, p);
            if (IsPrimitiveRoot(g, p))
            {
                return g;
            }
        }
    }
}
