
static partial class MyEnumerable
{
    public static IEnumerable<TResult> MySelect<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        Console.WriteLine("MySelect()");
        foreach (var item in source)
        {
            var result = selector(item);
            Console.WriteLine($"Select({item}):return > {result}");
            yield return result;
        }
    }

    public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        Console.WriteLine($"MyWhere()");

        foreach (var item in source)
        {
            if (predicate(item))
            {
                Console.WriteLine($"Where({item}):return > {item}");
                yield return item;
            }
            else
            {
                Console.WriteLine($"Where({item})");
            }
        }
    }

    public static IEnumerable<T> MySkip<T>(this IEnumerable<T> source, int count)
    {
        Console.WriteLine($"MySkip({count})");
        foreach (var item in source)
        {
            if (--count <= 0)
            {
                Console.WriteLine($"Skip({item}):return > {item}");
                yield return item;
            }
            else
            {
                Console.WriteLine($"Skip({item})");
            }
        }

    }
    public static IEnumerable<T> MyTake<T>(this IEnumerable<T> source, int count)
    {
        Console.WriteLine($"MyTake({count})");
        if (count > 0)
        {
            foreach (var item in source)
            {
                Console.WriteLine($"Take({item}):return > {item}");
                yield return item;
                if (--count == 0) break;
            }
        }
    }

    public static void MyForEach<T>(this IEnumerable<T> source)
    {
        Console.WriteLine("MyForEach()");
        var enumerator = source.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ConsoleEx.WriteLine(enumerator.Current, ConsoleColor.Green);
            }
        }
        finally
        {
            enumerator.Dispose();
        }
    }

    public static IList<T> MyToList<T>(this IEnumerable<T> source)
    {
        Console.WriteLine("MyToList()");
        var list = new List<T>();
        foreach (var item in source)
        {
            ConsoleEx.WriteLine($"ToList({item})", ConsoleColor.Green);
            list.Add(item);
        }
        return list;
    }

    public static int MyCount<T>(this IEnumerable<T> source)
    {
        Console.WriteLine($"MyCount()");
        var count = 0;
        foreach (var item in source)
        {
            count++;
            ConsoleEx.WriteLine($"Count({item}):count = {count}", ConsoleColor.Green);
        }
        return count;
    }

}
