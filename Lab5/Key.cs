namespace Lab5;

public class Key
{
    public readonly int E;
    public readonly int N;
    public int D => _keyType == KeyType.Private && _d.HasValue ? _d.Value : throw new ArgumentException();
    private readonly int? _d;
    private readonly KeyType _keyType;

    public Key(int n, int e)
    {
        N = n;
        _keyType = KeyType.Public;
        E = e;
    }

    public Key(int n, int d, int e)
    {
        N = n;
        _d = d;
        _keyType = KeyType.Private;
        E = e;
    }
}

public enum KeyType
{
    Public = 0,
    Private = 1,
}