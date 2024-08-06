using System.Collections;

class CountUpEnumerable(int start) : IEnumerable<int>
{
    public IEnumerator<int> GetEnumerator()
    {
        ConsoleEx.WriteLine("@ GetEnumerator()", ConsoleColor.DarkYellow);
        return new Enumerator(start);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private class Enumerator(int start) : IEnumerator<int>
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
}