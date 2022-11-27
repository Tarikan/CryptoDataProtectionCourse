namespace Lab6;

public class GF256
{
    public const int Poly = 0x11b;

    public static byte Mul02(byte b)
    {
        var b7 = b >> 7;
        return (byte)((b << 1) ^ (b7 * Poly));
    }

    public static byte Mul03(byte b)
    {
        return (byte)(Mul02(b) ^ b ^ 0);
    }
    
    public static byte Mul(byte b0, byte b1)
    {
        var product = 0;
        for (var i = 0; i < 8; i++)
        {
            product <<= 1;
            if ((product & 0x100) != 0)
            {
                product ^= Poly;
            }

            if ((b0 & 0x80u) != 0)
            {
                product ^= b1;
            }

            b0 <<= 1;
        }

        return (byte) product;
    }
}