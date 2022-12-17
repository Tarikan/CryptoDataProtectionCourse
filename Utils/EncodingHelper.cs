using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils;
public static class EncodingHelper
{
    public static byte[] EncodeWithAlign(string source, Encoding encoding, int align = 16)
    {
        var length = (source.Length + align - 1) / align * align;
        var result = new byte[length];
        encoding.GetBytes(source, result);

        return result;
    }
}
