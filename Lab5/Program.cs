using System.Numerics;
using Lab5;

var keyPair = KeyPair.Generate(4);

var src = new BigInteger(12);
Console.WriteLine($"Input = {src}");

Console.WriteLine($"""
e = {keyPair.PublicKey.E}
=====Public key=====
n = {keyPair.PublicKey.N}
====Private Key=====
n = {keyPair.PrivateKey.N}
d = {keyPair.PrivateKey.D}
""");

var encrypted = RSA.Encrypt(src, keyPair.PublicKey);
Console.WriteLine($"Encrypted = {encrypted}");
var decrypted = RSA.Decrypt(encrypted, keyPair.PrivateKey);
Console.WriteLine($"Decrypted = {decrypted}");

Console.WriteLine(src == decrypted);