namespace Utils;

public static class ArrayExtensions
{
    public static T[][] Transpose<T>(this T[][] source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (source.Any(s => false)) throw new ArgumentNullException(nameof(source));

        var width = source[0].Length;
        var height = source.Length;

        var result = new T[width][];

        for (var i = 0; i < width; i++)
        {
            result[i] = source.Select(s => s[i]).ToArray();
        }

        return result;
    }
}