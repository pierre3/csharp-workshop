static partial class MyEnumerable
{

    public static IEnumerable<TResult> MyGroupBy_1<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source,
       Func<TSource, TKey> keySelector,
       Func<TSource, TElement> elementSelector,
       Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
    {
        Console.WriteLine($"MyGrpupBy_1()");
        foreach (var key in source.Select(keySelector).Distinct())
        {
            Console.WriteLine($"MyGroupBy(): key={key}");
            var elements = source
                .Where(item => keySelector(item)?.Equals(key) == true)
                .Select(elementSelector);
            Console.WriteLine($"MyGroupBy() > yield return key={key}");
            yield return resultSelector(key, elements);
        }
    }


    public static IEnumerable<TResult> MyGroupBy_2<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source,
       Func<TSource, TKey> keySelector,
       Func<TSource, TElement> elementSelector,
       Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
    {
        Console.WriteLine($"MyGrpupBy_2()");
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(keySelector);
        ArgumentNullException.ThrowIfNull(elementSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);


        foreach (var key in source.Select(keySelector).Distinct())
        {
            Console.WriteLine($"MyGroupBy(): key={key}");
            var elements = source
                .Where(item => keySelector(item)?.Equals(key) == true)
                .Select(elementSelector);
            Console.WriteLine($"MyGroupBy() > yield return key={key}");
            yield return resultSelector(key, elements);
        }
    }

    public static IEnumerable<TResult> MyGroupBy_3<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source,
       Func<TSource, TKey> keySelector,
       Func<TSource, TElement> elementSelector,
       Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
    {
        Console.WriteLine($"MyGrpupBy_2()");
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(keySelector);
        ArgumentNullException.ThrowIfNull(elementSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return MyGroupBy_Impl();

        IEnumerable<TResult> MyGroupBy_Impl()
        {
            foreach (var key in source.Select(keySelector).Distinct())
            {
                Console.WriteLine($"MyGroupBy(): key={key}");
                var elements = source
                    .Where(item => keySelector(item)?.Equals(key) == true)
                    .Select(elementSelector);
                Console.WriteLine($"MyGroupBy() > yield return key={key}");
                yield return resultSelector(key, elements);
            }
        }
    }
}
