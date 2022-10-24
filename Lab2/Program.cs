using Lab2;

var algo = new Algo("абвгдеєжзиійклмнопрстуфхцчшьюя", 6);

var textToEncrypt = "заміна";

var encrypted = algo.Encrypt(textToEncrypt);

var decrypted = algo.Decrypt(encrypted);

Console.WriteLine(encrypted);
Console.WriteLine(decrypted);
Console.WriteLine(textToEncrypt == decrypted);
