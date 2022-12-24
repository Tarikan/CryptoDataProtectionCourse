using Lab8;
using System.Numerics;

Console.WriteLine("Diffie-Hellman test");

var Alice = new User();
Console.WriteLine($"Alice's secret number is {Alice.SecretRandomNumber}");
var Bob = new User();
Console.WriteLine($"Bob's secret number is {Bob.SecretRandomNumber}");

Alice.CommitDiffieHellman(Bob);

Console.WriteLine("ElGamal test");

var elGamal = new ElGamal();
Console.WriteLine($"p = {elGamal.p}|g = {elGamal.g}|x = {elGamal.x}|y = {elGamal.y}");

var m = new BigInteger(5);
Console.WriteLine($"message is: {m}");

var encrypted = elGamal.Encrypt(m);
Console.WriteLine($"Encrypted: a = {encrypted.a}, b = {encrypted.b}");
var decrypted = elGamal.Decrypt(encrypted);
Console.WriteLine($"Decrypted = {decrypted}");

Console.WriteLine($"Is initial value equal to decrypted: {m == decrypted}");
