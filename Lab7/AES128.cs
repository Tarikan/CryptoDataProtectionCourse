using Lab6;
using System.Security.Cryptography;

namespace Lab7;
public class AES128
{
    public const int AES_KEY_SIZE = 16;
    public const int AES_BLOCK_SIZE = 16;
    public const int AES_ROUNDS_NUMBER = 10;
    private byte[] _key;
    private byte[][] _roundKeys;

    public byte[] Key
    {
        get => _key;
        set {
            if (value == null || value.Length != AES_KEY_SIZE)
                return;
            _key = value;
            if (_roundKeys == null || _roundKeys.Length != AES_ROUNDS_NUMBER)
            {
                _roundKeys = Enumerable.Range(0, AES_ROUNDS_NUMBER + 1)
                    .Select(_ => new byte[AES_KEY_SIZE])
                    .ToArray();
            }

            Array.Copy(_key, _roundKeys[0], AES_KEY_SIZE);
            for (var i = 0; i < AES_ROUNDS_NUMBER; ++i)
            {
                Array.Copy(_roundKeys[i], _roundKeys[i + 1], AES_KEY_SIZE);
                GenerateRoundKey(_roundKeys[i + 1], i);
            }
        }
    }

    public byte[][] RoundKeys => _roundKeys;

    private static void GenerateRoundKey(byte[] key, int roundNumber)
    {
        byte[] keyword = new byte[4];
        Array.Copy(key, 12, keyword, 0, 4);

        var buf = keyword[0];
        keyword[0] = (byte)(Constants.sBox[keyword[1]] ^ Constants.roundCoefficientForAes128[roundNumber]);
        keyword[1] = Constants.sBox[keyword[2]];
        keyword[2] = Constants.sBox[keyword[3]];
        keyword[3] = Constants.sBox[buf];

        for (var k = 0; k < 4; ++k)
        {
            key[k] ^= keyword[k];
        }

        for (var i = 0; i < AES_KEY_SIZE - 4; ++i)
        {
            key[i + 4] ^= key[i];
        }
    }

    public static void ShiftRows(byte[] text)
    {
        var buffer = text[1];
        text[1] = text[5];
        text[5] = text[9];
        text[9] = text[13];
        text[13] = buffer;

        buffer = text[2];
        text[2] = text[10];
        text[10] = buffer;
        buffer = text[6];
        text[6] = text[14];
        text[14] = buffer;

        buffer = text[15];
        text[15] = text[11];
        text[11] = text[7];
        text[7] = text[3];
        text[3] = buffer;
    }
    public static void InverseShiftRows(byte[] text)
    {
        var buffer = text[1];
        text[1] = text[13];
        text[13] = text[9];
        text[9] = text[5];
        text[5] = buffer;

        buffer = text[2];
        text[2] = text[10];
        text[10] = buffer;
        buffer = text[6];
        text[6] = text[14];
        text[14] = buffer;

        buffer = text[15];
        text[15] = text[3];
        text[3] = text[7];
        text[7] = text[11];
        text[11] = buffer;
    }

    public static void MixColumn(byte[] text)
    {
        byte[] temp = new byte[4];
        int p;
        for (int i = 0; i < 4; ++i)
        {
            p = i * 4;
            temp[0] = (byte)(GF256.Mul02(text[p]) ^ (GF256.Mul02(text[p + 1]) ^ text[p + 1]) ^ text[p + 2] ^ text[p + 3]);
            temp[1] = (byte)(GF256.Mul02(text[p + 1]) ^ (GF256.Mul02(text[p + 2]) ^ text[p + 2]) ^ text[p + 3] ^ text[p]);
            temp[2] = (byte)(GF256.Mul02(text[p + 2]) ^ (GF256.Mul02(text[p + 3]) ^ text[p + 3]) ^ text[p] ^ text[p + 1]);
            temp[3] = (byte)(GF256.Mul02(text[p + 3]) ^ (GF256.Mul02(text[p]) ^ text[p]) ^ text[p + 1] ^ text[p + 2]);
            Array.Copy(temp, 0, text, p, 4);
        }
    }

    public static void InverseMixColumn(byte[] text)
    {
        var temp = new byte[4];
        int p;
        for (var i = 0; i < 4; ++i)
        {
            p = i * 4;
            temp[0] = (byte)(GF256.Mul(text[p], 14) ^ GF256.Mul(text[p + 1], 11) ^ GF256.Mul(text[p + 2], 13) ^ GF256.Mul(text[p + 3], 9));
            temp[1] = (byte)(GF256.Mul(text[p], 9) ^ GF256.Mul(text[p + 1], 14) ^ GF256.Mul(text[p + 2], 11) ^ GF256.Mul(text[p + 3], 13));
            temp[2] = (byte)(GF256.Mul(text[p], 13) ^ GF256.Mul(text[p + 1], 9) ^ GF256.Mul(text[p + 2], 14) ^ GF256.Mul(text[p + 3], 11));
            temp[3] = (byte)(GF256.Mul(text[p], 11) ^ GF256.Mul(text[p + 1], 13) ^ GF256.Mul(text[p + 2], 9) ^ GF256.Mul(text[p + 3], 14));
            Array.Copy(temp, 0, text, p, 4);
        }
    }

    private byte[] EncryptBlock(byte[] input)
    {
        var output = new byte[input.Length];
        input.CopyTo(output, 0);
        for (int j = 0; j < AES_BLOCK_SIZE; ++j)
            output[j] ^= RoundKeys[0][j];
        for (int r = 0; r < AES_ROUNDS_NUMBER; ++r)
        {
            for (int i = 0; i < AES_BLOCK_SIZE; ++i)
                output[i] = Constants.sBox[output[i]];
            ShiftRows(output);
            if (r < AES_ROUNDS_NUMBER - 1)
                MixColumn(output);
            for (int j = 0; j < AES128.AES_BLOCK_SIZE; ++j)
                output[j] ^= RoundKeys[r + 1][j];
        }
        return output;
    }

    private byte[] DecryptBlock(byte[] input)
    {
        var output = new byte[input.Length];
        input.CopyTo(output, 0);
        
        for (int r = 0; r < AES_ROUNDS_NUMBER; ++r)
        {
            for (int j = 0; j < AES128.AES_BLOCK_SIZE; ++j)
                output[j] ^= RoundKeys[AES_ROUNDS_NUMBER - r][j];
            if (r > 0)
                InverseMixColumn(output);
            InverseShiftRows(output);
            for (int i = 0; i < AES_BLOCK_SIZE; ++i)
                output[i] = Constants.inverseSBox[output[i]];

        }
        for (int j = 0; j < AES_BLOCK_SIZE; ++j)
            output[j] ^= RoundKeys[0][j];
        return output;
    }

    public byte[] Encrypt(byte[] input)
    {
        var result = new byte[input.Length];

        var blocksToEncrypt = input.Length / AES_BLOCK_SIZE;
        byte[] blockEncryptRes;
        var text = new byte[AES_BLOCK_SIZE];
        int p;
        for (int k = 0; k < blocksToEncrypt; ++k)
        {
            p = k * AES_BLOCK_SIZE;
            Array.Copy(input, p, text, 0, AES_BLOCK_SIZE);
            blockEncryptRes = EncryptBlock(text);
            Array.Copy(blockEncryptRes, 0, result, p, AES_BLOCK_SIZE);
        }

        return result;
    }

    public byte[] Decrypt(byte[] input)
    {
        var result = new byte[input.Length];

        var blocksToEncrypt = input.Length / AES_BLOCK_SIZE;
        byte[] blockEncryptRes;
        var text = new byte[AES_BLOCK_SIZE];
        int p;
        for (int k = 0; k < blocksToEncrypt; ++k)
        {
            p = k * AES_BLOCK_SIZE;
            Array.Copy(input, p, text, 0, AES_BLOCK_SIZE);
            blockEncryptRes = DecryptBlock(text);
            Array.Copy(blockEncryptRes, 0, result, p, AES_BLOCK_SIZE);
        }

        return result;
    }

}
