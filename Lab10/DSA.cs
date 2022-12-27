using Lab4;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Utils;

namespace Lab10;
public static class DSA
{
    private static readonly Random _random = new Random();
    private static readonly BigInteger minQLen = BigInteger.Pow(2, 160);

    public static DsaKeys GenerateKeys()
    {
        var L = _random.Next(8, 17) * 64;
        var p = RandomNumUtil.GenerateRandomPrimeWithSpecifiedBitLength(L);
        var q = (p - 1) / 2;
        var g = RandomNumberHelper.RandomIntegerInRange(1, p);
        var h = BigInteger.ModPow(g, (p - 1) / q, p);
        var a = RandomNumberHelper.RandomIntegerInRange(1, q);
        var b = BigInteger.ModPow(h, a, p);

        return new DsaKeys
        {
            p = p,
            q = q,
            h = h,
            a = a,
            b = b,
        };
    }

    public static Signature Sign(string message, DsaKeys keys)
    {
        var r = RandomNumberHelper.RandomIntegerInRange(0, keys.q);
        var r1 = ModularMath.InverseElement(r, keys.q);
        var s1 = BigInteger.ModPow(keys.h, r, keys.p) % keys.q;

        var hash = new BigInteger(ComputeSha256Hash(message));

        var s2 = (r1 * (hash + keys.a * s1)) % keys.q;

        return new Signature 
        {
            s1 = s1,
            s2 = s2,
        };
    }

    public static bool CheckSignature(string message, Signature signature, DsaKeys keys)
    {
        var s_ = ModularMath.InverseElement(signature.s2, keys.q);

        var hash = new BigInteger(ComputeSha256Hash(message));

        var u1 = (hash * s_) % keys.q;
        var u2 = (s_ * signature.s1) % keys.q;
        var t = ((BigInteger.ModPow(keys.h, u1, keys.p) * BigInteger.ModPow(keys.b, u2, keys.p)) % keys.p) % keys.q;

        return t == signature.s1;
    }

    private static byte[] ComputeSha256Hash(string rawData)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            return sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        }
    }
}

public class DsaKeys
{
    public BigInteger p { get; init; }
    public BigInteger q { get; init; }
    public BigInteger h { get; init; }
    public BigInteger b { get; init; }
    public BigInteger a { get; init; }
}

public class Signature
{
    public BigInteger s1 { get; init; }
    public BigInteger s2 { get; init; }
}