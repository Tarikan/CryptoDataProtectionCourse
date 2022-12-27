using Lab10;

var keys = DSA.GenerateKeys();

Console.WriteLine($""""
    Keys:
    p = {keys.p}
    q = {keys.q}
    h = {keys.h}
    b = {keys.b}
    a = {keys.a}
    """");

var input = "Hello World!";
Console.WriteLine($"Input: {input}");
var signature = DSA.Sign(input, keys);
Console.WriteLine($"""
    Signature:
    s1 = {signature.s1}
    s2 = {signature.s2}
    """);
var result = DSA.CheckSignature(input, signature, keys);
Console.WriteLine($"Signature check result: {result}");