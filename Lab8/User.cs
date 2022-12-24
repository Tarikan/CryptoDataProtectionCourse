using Lab5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lab8;
public class User
{
    public readonly BigInteger SecretRandomNumber;
    private readonly RandomNumberGenerator rnd = RandomNumberGenerator.Create();

    public User()
    {
        SecretRandomNumber = Utils.GenerateRandomBigInt(0);
    }

    public void CommitDiffieHellman(User user)
    {
        var p = Utils.GenerateRandomPrime();
        var g = Utils.GetG(p);

        Console.WriteLine($"p = {p}|g = {g}");

        var A = this.GetPublicRandomNumber(g, p);
        var B = user.GetPublicRandomNumber(g, p);

        Console.WriteLine($"A = {A}|B = {B}");
        var k1 = BigInteger.ModPow(B, this.SecretRandomNumber, p);
        var k2 = BigInteger.ModPow(A, user.SecretRandomNumber, p);

        Console.WriteLine($"User 1 key = {k1}|User 2 key = {k2}");
        Console.WriteLine($"Are keys the same: {k1 == k2}");
    }

    public BigInteger GetPublicRandomNumber(BigInteger g, BigInteger p)
    {
        return BigInteger.ModPow(g, SecretRandomNumber, p);
    }
}
