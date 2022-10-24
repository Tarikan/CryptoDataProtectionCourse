using Utils;

namespace Lab1;

public class Algo
{
    public readonly string ColumnKey;

    public readonly string RowKey;

    private readonly int maxLength;

    private readonly int[] DechiperColumnKey;
    
    private readonly int[] DechiperRowKey;

    public Algo(string columnKey, string rowKey, char defaultValue = '.')
    {
        ColumnKey = columnKey;
        RowKey = rowKey;
        maxLength = columnKey.Length * rowKey.Length;
        
        DechiperColumnKey = columnKey
            .Select((x, i) => new KeyValuePair<int , char>(i, x))
            .OrderBy(k => k.Value)
            .Select(x => x.Key)
            .ToArray();
        
        DechiperRowKey = RowKey
            .Select((x, i) => new KeyValuePair<int , char>(i, x))
            .OrderBy(k => k.Value)
            .Select(x => x.Key)
            .ToArray();
    }

    public string Encrypt(string value)
    {
        if (value.Length > maxLength) throw new ArgumentException(nameof(value));

        value = value.PadRight(maxLength);

        var matrix = CreateMatrix(value);

        var sortedByRow = matrix
            .Zip(RowKey)
            .OrderBy(a => a.Second)
            .Select(a => a.First)
            .ToArray();

        var transposed = sortedByRow.Transpose();

        var sortedByCol = transposed
            .Zip(ColumnKey)
            .OrderBy(a => a.Second)
            .Select(a => a.First)
            .ToArray();

        return string.Join("", sortedByCol.SelectMany(i => i));
    }

    public string Decrypt(string value)
    {
        if (value.Length > maxLength) throw new ArgumentException(nameof(value));

        var matrix = CreateMatrix(value, true);
        
        var decipheredByColumn = matrix
            .Zip(DechiperColumnKey)
            .OrderBy(a => a.Second)
            .Select(a => a.First)
            .ToArray();
        
        var transposed = decipheredByColumn.Transpose();
        
        var decipheredByRow = transposed
            .Zip(DechiperRowKey)
            .OrderBy(a => a.Second)
            .Select(a => a.First)
            .ToArray();

        return string.Join("", decipheredByRow.SelectMany(a => a));
    }

    private char[][] CreateMatrix(string value, bool invert = false)
    {
        var width = invert ? RowKey.Length : ColumnKey.Length;
        var height = invert ? ColumnKey.Length : RowKey.Length;
        
        var result = new char[height][];

        var valueAsArray = value.ToArray();
        for (var i = 0; i < height; i++)
        {
            result[i] = valueAsArray.Skip(i * width).Take(width).ToArray();
        }

        return result;
    }
}