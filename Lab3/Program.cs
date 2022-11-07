using System.Text;
using Lab3;
using Utils;

var key = Encoding.UTF8.GetBytes("abcdefgh");
var value = Encoding.UTF8.GetBytes("ABCDEFGH");

Console.WriteLine("Input: ABCDEFGH\n");
Console.WriteLine($"Encrypt | Value: {PrintHelper.ToFancyByteArray(value)} | Key: {PrintHelper.ToFancyByteArray(key)}");
var encrypted = Algo.Crypt(value, key, true);
Console.WriteLine($"Encrypt result: {PrintHelper.ToFancyByteArray(encrypted)}");

Console.WriteLine();

Console.WriteLine($"Decrypt | Value: {PrintHelper.ToFancyByteArray(encrypted)} | Key: {PrintHelper.ToFancyByteArray(key)}");
var decrypted = Algo.Crypt(encrypted, key, false);
Console.WriteLine($"Decrypt result: {PrintHelper.ToFancyByteArray(decrypted)}");

Console.WriteLine($"\nResult: {Encoding.UTF8.GetString(decrypted)}");