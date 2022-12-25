using lab9;
using System.Text;

var input = "Hello world!";
var hash = MD5.Calculate(Encoding.ASCII.GetBytes(input));

Console.WriteLine($"Input: {input}");
Console.WriteLine($"hash: {hash}");
