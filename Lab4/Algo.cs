namespace Lab4;

public class Algo
{
    public static (int d, int x, int y) GcdEx(int a, int b)
    {
        if (a == 0)
        {
            return (b, 0, 1);
        }

        var a0 = a;
        var a1 = b;
        var x0 = 1;
        var x1 = 0;
        var y0 = 0;
        var y1 = 1;

        while (a1 != 0)
        {
            var q = a0 / a1;

            (a0, a1) = (a1, a0 - q * a1);
            (x0, x1) = (x1, x0 - q * x1);
            (y0, y1) = (y1, y0 - q * y1);
        }

        return (a0, x0, y0);
    }

    public static int InverseElement(int a, int n)
    {
        var (g, x, y) = GcdEx(a, n);

        var res = (x % n + n) % n;

        return res;
    }

    public static int Phi(int n)
    {
        var result = 1;
        for (var i = 2; i < n; i++)
            if (GcdEx(i, n).d == 1)
                result++;
        return result;
    }

    public static int InverseElementByPhi(int a, int n)
    {
        return IsPrime(n) ? PowModulo(a, n - 2, n) : PowModulo(a, Phi(n) - 1, n);
    }

    private static bool IsPrime(int n)
    {
        for (var i = 2; i < n; i++)
            if (n % i == 0)
                return false;
        return true;
    }

    private static int PowModulo(int x, int y, int n)
    {
        if (y == 0)
            return 1;

        var p = PowModulo(x, y / 2, n) % n;
        p = p * p % n;

        return y % 2 == 0 ? p : x * p % n;
    }
}