using System.Collections;

/// <summary>
/// 値を１ずつ加算する列挙子により列挙可能なクラス
/// </summary>
/// <param name="start">初期値</param>
class CountUpEnumerable(int start) : IEnumerable<int>
{
    public IEnumerator<int> GetEnumerator()
    {
        ConsoleEx.WriteLine("@ GetEnumerator()", ConsoleColor.DarkYellow);
        return new CountUpEnumerator(start);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// 値を１ずつ加算する列挙子
/// </summary>
/// <param name="start">初期値</param>
class CountUpEnumerator(int start) : IEnumerator<int>
{
    private int _value = start;
    public int Current { get; private set; }

    public void Dispose()
    {
        ConsoleEx.WriteLine($"@ Dispose()", ConsoleColor.DarkYellow);
    }

    public bool MoveNext()
    {
        Current = _value;
        ConsoleEx.WriteLine($"@ MoveNext() > Current={Current}", ConsoleColor.DarkYellow);
        _value++;
        return true;
    }

    object IEnumerator.Current => Current;
    public void Reset() => throw new NotImplementedException();
}