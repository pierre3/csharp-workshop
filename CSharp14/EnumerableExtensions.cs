using System.Numerics;

namespace CSharp14;

public static class EnumerableExtensions
{
    public static string Dump<T>(this IEnumerable<T> source)
    {
        return string.Join(", ", source);
    }

    extension(string source)
    {
        public string Quoted => $"\"{source}\"";
    }

    extension<T>(IEnumerable<T> source)
    {
        public bool IsEmpty => !source.Any();
        public T this[int index] => source.Skip(index).First();
    }

    extension<T>(IEnumerable<T>) where T : INumber<T>
    {
        public static IEnumerable<T> CountUp(T start)
        {
            for (T i = start; ; i++)
            {
                yield return i;
            }
        }
    }
}
