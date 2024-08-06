using System.Collections;


class CountUpIterator : IEnumerator<int>, IEnumerable<int>
{
    private int _state;
    private int _start;
    public int Current { get; private set; }

    public CountUpIterator(int start)
    {
        _start = start;
        ConsoleEx.WriteLine($"@ new {nameof(CountUpIterator)}()", ConsoleColor.DarkYellow);
    }

    public IEnumerator<int> GetEnumerator()
    {
        ConsoleEx.WriteLine($"@ GetEnumerator()", ConsoleColor.DarkYellow);
        var e = _state == 0 ? this : new CountUpIterator(_start);
        e._state = 1;
        return e;
    }

    public bool MoveNext()
    {
        switch (_state)
        {
            case 1:
                Current = _start;
                _state = 2;
                ConsoleEx.WriteLine($"@ MoveNext() > {Current}", ConsoleColor.DarkYellow);
                return true;
            case 2:
                Current++;
                ConsoleEx.WriteLine($"@ MoveNext() > {Current}", ConsoleColor.DarkYellow);
                return true;
        }
        Dispose();
        return false;
    }

    public void Dispose()
    {
        _state = -1;
        ConsoleEx.WriteLine("@ Dispose()", ConsoleColor.DarkYellow);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    object IEnumerator.Current => Current;
    public void Reset() => throw new NotImplementedException();

}

