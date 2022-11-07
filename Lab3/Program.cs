using System.Text;
using Lab3;

var key = Encoding.UTF8.GetBytes("abcdefgh");
var value = Encoding.UTF8.GetBytes("ABCDEFGH");

var encrypted = Algo.Crypt(value, key, true);

var decrypted = Algo.Crypt(encrypted, key, false);

Console.WriteLine(Encoding.UTF8.GetString(decrypted));