namespace Lab2;

public class Algo
{
    public readonly string CharSet;

    private readonly int width;
    private readonly int height;

    private readonly char[][] matrix;

    public Algo(string charSet, int width)
    {
        CharSet = charSet;
        this.width = width;
        height = charSet.Length / width;

        matrix = new char[height][];

        for (var i = 0; i < height; i++)
        {
            matrix[i] = charSet.Skip(i * width).Take(width).ToArray();
        }
    }

    public string Encrypt(string source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var positions = new List<(int, int)>();
        foreach (var character in source)
        {
            var i = matrix
                .Select((a, idx) => (a, idx))
                .Where(tuple => tuple.a.Contains(character))
                .Select(tuple => tuple.idx)
                .Single();
            var j = matrix[i]
                .Select((a, idx) => (a, idx))
                .Where(tuple => tuple.a == character)
                .Select(tuple => tuple.idx)
                .Single();
            positions.Add((i, j));
        }

        var numbers = positions.Select(res => res.Item1).ToList();
        numbers.AddRange(positions.Select(res => res.Item2));

        var result = new List<string>();
        for (var i = 0; i < source.Length; i++)
        {
            result.Add(string.Join("", numbers.Skip(i * 2).Take(2)));
        }

        return string.Join(" ", result);
    }

    public string Decrypt(string source)
    {
        var numbers = source
            .Split(" ")
            .SelectMany(n => n)
            .Select(n => int.Parse(n.ToString()))
            .ToList();

        var indexes = numbers
            .Chunk(numbers.Count / 2)
            .ToList();

        return string.Join("", indexes[0]
            .Zip(indexes[1])
            .Select(idx => matrix[idx.First][idx.Second]));
    }
}