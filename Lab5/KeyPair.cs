using Lab4;

namespace Lab5;

public class KeyPair
{
    public readonly Key PublicKey;

    public readonly Key PrivateKey;

    private KeyPair(int n, int d, int e)
    {
        PublicKey = new Key(n, e);
        PrivateKey = new Key(n, d, e);
    }

    public static KeyPair Generate(int bitLength)
    {
        var (p, q) = FindPrimeNumbers(bitLength);
        Console.WriteLine($"p = {p} | q = {q}");

        var n = p * q;
        var phi = (p - 1) * (q - 1);

        var e = 2;

        while (e < phi) {
            if (ModularMath.GcdEx(e, phi).d == 1)
                break;
            e++;
        }

        var d = ModularMath.InverseElementByPhi(e, phi);

        return new KeyPair(n, d, e);
    }

    public static (int, int) FindPrimeNumbers(int bitLength)
    {
        var first = - 1;

        if (bitLength is <= 0 or > 32)
        {
            throw new ArgumentException(nameof(bitLength));
        }

        var random = new Random();
        var randomBytes = new byte[(bitLength + 8 - 1) / 8];
        random.NextBytes(randomBytes);
        if (randomBytes.Length != 4)
        {
            randomBytes = randomBytes.Concat(Enumerable.Repeat(default(byte), 4 - randomBytes.Length)).ToArray();
        }
        var randomNum = BitConverter.ToInt32(randomBytes, 0);

        while (true)
        {
            if (RabinMiller.IsPrime(randomNum, 40))
            {
                if (first is -1)
                {
                    first = randomNum;
                    randomNum += 1;
                    continue;
                }

                var second = randomNum;
                return (first, second);
            }

            randomNum += 1;
        }
    }
}