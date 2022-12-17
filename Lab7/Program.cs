using Lab7;
using System.Text;
using Utils;

var random = new Random();
var test = new byte[16];
var test1 = new byte[16];

// MixColumn and InverseMixColumn check
random.NextBytes(test);
Array.Copy(test, test1, 16);
AES128.MixColumn(test1);
AES128.InverseMixColumn(test1);
Console.WriteLine($"Check for MixColumn and InverseMixColumn {Enumerable.SequenceEqual(test, test1)}");

// ShiftRows and InverseShiftRows check
random.NextBytes(test);
Array.Copy(test, test1, 16);
AES128.ShiftRows(test1);
AES128.InverseShiftRows(test1);
Console.WriteLine($"Check for ShiftRows and InverseShiftRows {Enumerable.SequenceEqual(test, test1)}");

// Encrypt and decrypt check
var aes = new AES128();
var key = new byte[16];
random.NextBytes(key);
Console.WriteLine($"Key {Encoding.ASCII.GetString(key)}");
aes.Key = key;
var input = EncodingHelper.EncodeWithAlign("Hello world", Encoding.ASCII, 16);
Console.WriteLine($"Input string {Encoding.ASCII.GetString(input)}");
var encrypted = aes.Encrypt(input);
Console.WriteLine($"Encrypted string {Encoding.ASCII.GetString(encrypted)}");
var decrypted = aes.Decrypt(encrypted);
Console.WriteLine($"Decrypted string {Encoding.ASCII.GetString(decrypted)}");