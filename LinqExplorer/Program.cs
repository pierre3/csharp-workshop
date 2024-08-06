using ConsoleAppFramework;

var app = ConsoleApp.Create();

// LINQ をサポートする C# の機能
// https://learn.microsoft.com/ja-jp/dotnet/csharp/linq/get-started/features-that-support-linq
// - 暗黙的に型指定された変数 (var)
// - オブジェクト初期化子とコレクション初期化子
// - 匿名型
// - 拡張メソッド
// - ラムダ式
// - データとしての式


//1. シーケンスの反復処理を司るインターフェイス IEnumerable<T> と IEnumerator<T>
app.Add("sample-1", () =>
{
    ConsoleEx.WriteLine("# IEnumerable<T> と IEnumerator<T>", ConsoleColor.Magenta);
    foreach (var n in new CountUpEnumerable(1))
    {
        ConsoleEx.WriteLine(n, ConsoleColor.Green);
        if (n == 10) { break; }
    }

    ConsoleEx.WriteLine("# foreachの実装", ConsoleColor.Magenta);
    new CountUpEnumerable(1).Take(10).MyForEach();

});

//2. イテレータ
// シーケンスの反復処理はIEnumerable<T>とIEnumerator<T>の両方を実装したIterator<T>というオブジェクトによって実現されている。
// https://github.com/dotnet/runtime/blob/main/src/libraries/System.Linq/src/System/Linq/Iterator.cs
app.Add("sample-2", () =>
{
    ConsoleEx.WriteLine("# イテレータの実装例", ConsoleColor.Magenta);
    new CountUpIterator(1).Take(10).MyForEach();

    ConsoleEx.WriteLine("# イテレーター構文(yield return)による実装の簡略化", ConsoleColor.Magenta);
    Console.WriteLine("IEnumerator<T>を戻り値とするパターン");
    new CountUpIterator2(1).Take(10).MyForEach();

    Console.WriteLine("IEnumerable<T>を戻り値とするパターン");
    MyEnumerable.CountUp(1).Take(10).MyForEach();
});


//3. where, selectの実装と遅延実行
app.Add("sample-3", () =>
{
    Console.WriteLine("set query");
    var ie = new CountUpIterator(1)
        .MyWhere(n => n % 2 == 0)
        .MySelect(n => n * n)
        .MyTake(5);

    Console.WriteLine(">>start foreach");
    ie.MyForEach();
    Console.WriteLine("<< end foreach");
});

//4. count,toListの実装と即時実行
app.Add("sample-4", () =>
{
    ConsoleEx.WriteLine("# Count() と ToList()", ConsoleColor.Magenta);
    var ie = new CountUpIterator(1)
        .MyWhere(n => n % 2 == 0)
        .MySelect(n => n * n)
        .MyTake(5);

    Console.WriteLine("Count()");
    var count = ie.MyCount();
    ConsoleEx.WriteLine($"Count={count}", ConsoleColor.Green);

    Console.WriteLine("ToList()");
    var list = ie.MyToList();
    ConsoleEx.WriteLine($"[{string.Join(",", list)}]", ConsoleColor.Green);
});

//4. 即時実行されるメソッドを使うタイミング
app.Add("sample-5", () =>
{
    ConsoleEx.WriteLine("# ToList()で実体化した後にList<T>のCount()を使う", ConsoleColor.Magenta);
    var ie = new CountUpIterator(1)
        .MyWhere(n => n % 2 == 0)
        .MySelect(n => n * n)
        .MyTake(5);

    Console.WriteLine("ToList()");
    var list2 = ie.MyToList();
    ConsoleEx.WriteLine($"Count= {list2.Count}", ConsoleColor.Green);
    ConsoleEx.WriteLine($"[{string.Join(",", list2)}]", ConsoleColor.Green);
});

//6. GroupByの実装
app.Add("sample-6", () =>
{


    var groups = Student.GetSampleData().MyGroupBy_1(
        item => item.Class,
        item => item.Score,
        (key, scores) =>
        {
            Console.WriteLine($"ResultSelector: Key={key}");
            Console.WriteLine($"- Count");
            var count = scores.Count();
            Console.WriteLine($"- Max");
            var max = scores.Max();
            Console.WriteLine($"- Avarage");
            var avg = scores.Average();
            return new
            {
                Class = key,
                Count = count,
                Max = max,
                Average = avg
            };
        });

    foreach (var group in groups)
    {
        ConsoleEx.WriteLine(group, ConsoleColor.Green);
    }
});

//7. 引数チェックとイテレータ
app.Add("sample-7", () =>
{
    ConsoleEx.WriteLine("# 引数チェック(ローカル関数なし)", ConsoleColor.Magenta);
    Func<string, IEnumerable<int>, string>? resultSelector = null;
    try
    {
        Console.WriteLine(">> set query");
        var groups = Student.GetSampleData()
            .MyGroupBy_2(
                item => item.Class,
                item => item.Score,
                resultSelector);

        Console.WriteLine("...");
        Console.WriteLine("...");
        Console.WriteLine(">> start foreach");
        foreach (var group in groups)
        {
            ConsoleEx.WriteLine(group ?? "", ConsoleColor.Green);
        }
    }
    catch (ArgumentNullException e)
    {
        ConsoleEx.WriteLine(e.Message, ConsoleColor.Red);
    }

    ConsoleEx.WriteLine("# 引数チェック(ローカル関数あり)", ConsoleColor.Magenta);
    try
    {
        Console.WriteLine(">> set query");
        var groups = Student.GetSampleData()
            .MyGroupBy_3(
                item => item.Class,
                item => item.Score,
                resultSelector);

        Console.WriteLine("...");
        Console.WriteLine("...");
        Console.WriteLine(">> start foreach");
        foreach (var group in groups)
        {
            ConsoleEx.WriteLine(group ?? "", ConsoleColor.Green);
        }
    }
    catch (ArgumentNullException e)
    {
        ConsoleEx.WriteLine(e.Message, ConsoleColor.Red);
    }


});


app.Run(args);

