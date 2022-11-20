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
}