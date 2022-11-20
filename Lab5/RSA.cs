using System.Numerics;

namespace Lab5;

public class RSA
{
    public static BigInteger Encrypt(BigInteger src, Key publicKey)
    {
        return BigInteger.Pow(src, publicKey.E) % publicKey.N;
    }

    public static BigInteger Decrypt(BigInteger src, Key privateKey)
    {
        return BigInteger.Pow(src, privateKey.D) % privateKey.N;
    }
}