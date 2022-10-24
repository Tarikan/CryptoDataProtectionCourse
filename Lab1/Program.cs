using Lab1;

var algo = new Algo("крипто", "шифр");

var textToEncrypt = "програмнезабезпечення";

var encrypted = algo.Encrypt(textToEncrypt);

var decrypted = algo.Decrypt(encrypted);

Console.WriteLine(encrypted);
Console.WriteLine(decrypted);

Console.WriteLine(textToEncrypt == decrypted.Trim());