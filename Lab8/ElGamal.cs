using System.Numerics;

namespace Lab8;
internal class ElGamal
{
    public BigInteger p;
    public BigInteger g;
    public BigInteger y;
    public BigInteger x;

    public ElGamal()
    {
        p = Utils.GenerateRandomPrime();
        g = Utils.GetG(p);
        x = Utils.GenerateRandomBigInt(2, p - 1);
        y = BigInteger.ModPow(g, x, p);
    }
    public (BigInteger a, BigInteger b) Encrypt(BigInteger message)
    {
        var k = Utils.GenerateRandomBigInt(2, p - 2);
        Console.WriteLine($"k = {k}");
        var a = BigInteger.ModPow(g, k, p);
        var b = (BigInteger.ModPow(y, k, p) * message) % p;

        return (a, b);
    }

    public BigInteger Decrypt((BigInteger a, BigInteger b) encodedMessage)
    {
        var (a, b) = encodedMessage;
        a = BigInteger.ModPow(encodedMessage.a, x, p);
        a = BigInteger.ModPow(a, p - 2, p);

        return a * b % p;
    }
}
