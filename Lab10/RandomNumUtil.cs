using Lab5;
using System.Numerics;
using Utils;

namespace Lab10;
public static class RandomNumUtil
{
    public static BigInteger GenerateRandomPrimeWithSpecifiedBitLength(int bitNum)
    {
        var max = BigInteger.Pow(2, bitNum + 1);
        var min = BigInteger.Pow(2, bitNum);

        BigInteger randomBigInt;
        do
        {
            randomBigInt = RandomNumberHelper.RandomIntegerInRange(min, max);
        } while (!RabinMiller.IsProbablyPrime(randomBigInt, 10));

        return randomBigInt;
    }
}
