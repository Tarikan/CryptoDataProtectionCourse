using Lab6;

byte a = 0xD4;
byte b = 0xBF;

Console.WriteLine(GF256.Mul(a, 0x02).ToString("X2"));
Console.WriteLine(GF256.Mul(b, 0x03).ToString("X2"));
Console.WriteLine(GF256.Mul02(a).ToString("X2"));
Console.WriteLine(GF256.Mul03(b).ToString("X2"));