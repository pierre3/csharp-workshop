using System.Collections;

/// <summary>
/// イテレータ構文: IEnumerator<T>を返すメソッドの例
/// </summary>
class CountUpEnumerable2(int start) : IEnumerable<int>
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

/// <summary>
/// イテレータ構文: IEnumerable<T>を返すメソッドの例
/// </summary>
static partial class MyEnumerable
{
    public static IEnumerable<int> CountUp(int start)
    {
        ConsoleEx.WriteLine($"@ CountUp({start})", ConsoleColor.DarkYellow);
        var value = start;
        while (true)
        {
            ConsoleEx.WriteLine($"@ CountUp({value}):return > {value}", ConsoleColor.DarkYellow);
            yield return value++;
        }
    }
}