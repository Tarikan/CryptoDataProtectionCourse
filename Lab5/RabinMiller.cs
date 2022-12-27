using System.Numerics;
using System.Security.Cryptography;
using Utils;

namespace Lab5;

public static class RabinMiller
{
    public static bool IsPrime(int n, int k)
    {
        if (n < 2 || n % 2 == 0) return n == 2;

        var s = n - 1;
        while (s % 2 == 0)
        {
            s >>= 1;
        }

        var r = new Random();
        for (var i = 0; i < k; i++)
        {
            var a = r.Next(n - 1) + 1;
            var temp = s;
            long mod = 1;
            for (var j = 0; j < temp; ++j)  mod = (mod * a) % n;
            while (temp != n - 1
                   && mod != 1
                   && mod != n - 1)
            {
                mod = mod * mod % n;
                temp *= 2;
            }

            if (mod != n - 1 && temp % 2 == 0)
            {
                return false;
            }
        }
        return true;
    }

    public static bool IsPrime(BigInteger n, int k)
    {
        if (n < 2 || n % 2 == 0)
            return n == 2;

        var s = n - 1;
        while (s % 2 == 0)
        {
            s >>= 1;
        }

        var r = new Random();
        for (var i = 0; i < k; i++)
        {
            var a = RandomNumberHelper.RandomIntegerInRange(0, n - 1) + 1;
            var temp = s;
            BigInteger mod = 1;
            for (var j = 0; j < temp; ++j)
                mod = (mod * a) % n;
            while (temp != n - 1
                   && mod != 1
                   && mod != n - 1)
            {
                mod = mod * mod % n;
                temp *= 2;
            }

            if (mod != n - 1 && temp % 2 == 0)
            {
                return false;
            }
        }
        return true;
    }

    public static bool IsProbablyPrime(this BigInteger value, int witnesses = 10)
    {
        var random = new Random();
        if (value <= 1)
            return false;

        if (witnesses <= 0)
            witnesses = 10;

        var d = value - 1;
        var s = 0;

        while (d % 2 == 0)
        {
            d /= 2;
            s += 1;
        }

        byte[] bytes = new Byte[value.ToByteArray().LongLength];
        BigInteger a;

        for (int i = 0; i < witnesses; i++)
        {
            do
            {
                random.NextBytes(bytes);

                a = new BigInteger(bytes);
            }
            while (a < 2 || a >= value - 2);

            BigInteger x = BigInteger.ModPow(a, d, value);
            if (x == 1 || x == value - 1)
                continue;

            for (int r = 1; r < s; r++)
            {
                x = BigInteger.ModPow(x, 2, value);

                if (x == 1)
                    return false;
                if (x == value - 1)
                    break;
            }

            if (x != value - 1)
                return false;
        }

        return true;
    }
}