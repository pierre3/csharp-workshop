using System.Collections;

class CountUpIterator2(int start) : IEnumerable<int>
{
    public IEnumerator<int> GetEnumerator()
    {
        ConsoleEx.WriteLine($"@ CountUp({start})", ConsoleColor.DarkYellow);
        var value = start;
        while (true)
        {
            ConsoleEx.WriteLine($"@ CountUp({value}): yield return > {value}", ConsoleColor.DarkYellow);
            yield return value++;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

static partial class MyEnumerable
{
    public static IEnumerable<int> CountUp(int initialValue)
    {
        ConsoleEx.WriteLine($"@ CountUp({initialValue})", ConsoleColor.DarkYellow);
        var value = initialValue;
        while (true)
        {
            var value0 = value++;
            ConsoleEx.WriteLine($"@ CountUp({value0}):return > {value}", ConsoleColor.DarkYellow);

            yield return value;
        }
    }
}