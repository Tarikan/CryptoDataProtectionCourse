using System.Text;

namespace Utils;

public class PrintHelper
{
    public static string ToFancyByteArray(uint src)
    {
        var bytes = BitConverter.GetBytes(src);

        return ToFancyByteArray(bytes);
    }
    
    public static string ToFancyByteArray(byte[] src)
    {
        var sb = new StringBuilder();
        sb.Append("{ ");
        sb.Append(string.Join(", ", src.Select(b => $"0x{b:X2}")));
        sb.Append(" }");

        return sb.ToString();
    }
}