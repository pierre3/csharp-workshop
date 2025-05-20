using System.Collections.Generic;
namespace CSharp14;

static class FirstClassSpan
{
    static void M1<T>(this IEnumerable<T> enu) {; }
    static void M2<T>(this Span<T> span) {; }

    internal static void M()
    {
        //https://ufcpp.net/blog/2025/1/first-class-span/

        var intArray = new int[] { 1, 2, 3 };
        // これは以前からできた
        intArray.M1();
        // これができるようになった
        intArray.M2();

        
        // これはできない(ビルドエラー）
        //List<object> list = new List<string>();
        //Span<object> span1 = new Span<string>();
        
        // これができるようになった
        ReadOnlySpan<object> span2 = new ReadOnlySpan<string>();
    }
}