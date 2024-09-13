# LINQ

## 1. LINQとは
>統合言語クエリ (LINQ) は、C# 言語への直接的なクエリ機能の統合に基づくテクノロジのセットの名前です。 これまでは、データに対するクエリは、コンパイル時の型チェックや IntelliSense のサポートがない単純な文字列として表現されてきました。 さらに、SQL データベース、XML ドキュメント、さまざまな Web サービスなど、各種データ ソースの異なるクエリ言語を学習する必要があります。 LINQ では、クエリは、クラス、メソッド、イベントと同様に、ファースト クラスの言語コンストラクトです。。

[統合言語クエリ (LINQ):Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/csharp/linq/)


LINQの主な特徴は以下の通りです：
- 統一的な構文：LINQは、配列、リスト、XML、データベースなど、さまざまなデータソースに対して一貫したクエリ操作を提供します。
- 強い型付け：LINQクエリはコンパイル時に型チェックされるため、型ミスなどのエラーを早期に検出できます。
- IntelliSense サポート：Visual StudioのIntelliSenseはLINQクエリをサポートしており、クエリの作成を容易にします。



### 1.1 LINQプロバイダー
LINQを使用すると、データベース、XML、コレクションなど、さまざまなデータソースに対して一貫した方法でクエリを実行することが可能です。以下に、いくつかの主要なLINQプロバイダーを示します。

- LINQ to SQL
- LINQ to XML 
- LINQ to JSON
- LINQ to Objects

LINQ to Objectsは、C#のコレクション（配列やListなど）に対してクエリを実行します。このテキストでは、主にLINQ to Objectsについて解説します。

[C# での LINQ クエリの概要:Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/csharp/linq/get-started/introduction-to-linq-queries)

### 1.2 クエリ構文とメソッド構文
LINQの記述方法にはクエリ構文とメソッド構文があります。
1. クエリ構文はSQLに似た構文でクエリを記述します。

```cs
int[] numbers = { 5, 10, 8, 3, 6, 12 };
var evenNumbers = from number in numbers
    where number % 2 == 0
    orderby number
    select number;
```

2. メソッド構文はメソッド（拡張メソッド）を使ってメソッドチェーンでクエリを記述します。
```cs
int[] numbers = { 5, 10, 8, 3, 6, 12 };
var evenNumbers = numbers
    .Where(x => x % 2 == 0)
    .OrderBy(x => x).ToList();
```

クエリ構文はシンプルで直観的に分かりやすいとされています（特にSQLに慣れている人にとって）。しかし、以下の理由から、必ずしもクエリ構文を覚える必要はありません。

- クエリ構文はSQLに似て非なるものです。特殊な構文の記述方法を覚える必要があります。
- クエリ構文はコンパイル時にメソッド構文に変換されます。つまり、クエリ構文でできることは全てメソッド構文でできますが、メソッド構文に対応したクエリ構文は存在しない場合があります。

## 2. LINQをサポートするC#の機能

[LINQ をサポートする C# の機能:Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/csharp/linq/get-started/features-that-support-linq)

### 2.1 デリゲートとラムダ式

#### デリゲートとは
デリゲートは、関数を参照するための特別な機能です。これを使用すると、関数を変数やメソッドの引数として扱うことが可能になります。
デリゲートは、classやstruct、enumと同じく、型の一種として扱われます。メソッドの引数と戻り値を指定して定義します。

```cs
//デリゲートの定義
public delegate string IntToString(int n);

class A
{
    private string MethodA(int n)
    {
        return n.ToString();
    }

    public void Sample()
    {
        //指定した引数と戻り値が一致するメソッドを代入できる
        IntToString intToString = new IntToString(MethodA);
        //コンストラクタを省略して下記のような記述でも可
        IntToString intToString = MethodA;
    }
}
```

C#ではジェネリックを使った汎用デリゲート（`Action<..>`と`Func<..>`）が定義されており、delegateを自前で定義する機会はあまりありません。

- Action(戻り値がないもの)
```cs
public delegate void Action<in T>(T arg);
public delegate void Action<in T1, in T2>(T1 arg1, T2 arg2);
public delegate void Action<in T1, in T2, in T3>(T1 arg1, T2 arg2, T3 arg3);
...
```

- Func(戻り値があるもの)
```cs
public delegate TResult Func<in T, out TResult>(T arg);
public delegate TResult Func<in T1, in T2 out TResult>(T1 arg1, T2 arg2);
public delegate TResult Func<in T1, in T2, in T3 out TResult>(T1 arg1, T2 arg2, T3 arg3);
...
```

#### 匿名メソッド式
匿名メソッド式は、必要な場所で直接メソッドを定義するための機能です。関数を引数として渡す際や、一度しか使用しない関数を定義する際に有用です。

```cs
class A
{
    //個別のメソッド定義は不要
    //private string MethodA(int n)
    //{
    //    return n.ToString();
    //}

    public void Sample()
    {
        //その場で定義できる
        IntToString intToString = delegate(int n)
        { 
            return n.ToString
        };
    }
}
```

#### ラムダ式
ラムダ式は、匿名メソッド式から省略可能な部分を省き、さらに簡略化したものです。

```cs
class A
{
    public void Sample()
    {
        //delegeteキーワードを省略
        //引数の型が推論できる場合は省略可
        //引数が1つの場合、()の省略が可能
        //メソッドの本体が単一式（1行で書ける）の場合は {} と return　が省略可能
        IntToString intToString = n => n.ToString;
    }
}
```

参考: [デリゲートとラムダ式のサンプルコード](/LinqExplorer/MyApp.Delegate.cs)

### 2.2 オブジェクト初期化子とコレクション初期化子
オブジェクト初期化子を利用すると、単一式でインスタンスの生成とプロパティの初期化が同時に行えます。

```cs
class Student
{
  public int Id {get; set;}
  public string Name {get; set;}
  public string Class {get; set;}
  public int Score {get; set;}
}

//オブジェクト初期化子を使用しない場合
//コンストラクタ呼出し後、複数のステートメントに分けてプロパティに値を設定する必要があります。
var studentA = new Student();
student.Id = 1;
student.Name = "John Smith";
student.Class = "A"
student.Score = 70;

//オブジェクト初期化子を使用した場合
//単一式でインスタンスの生成とプロパティの初期化が同時に行えます。
var studentB = new Student {Id = 1, Name = "John Smith", Class = "A", Score = 70 };
```

```cs
//コレクション初期化子を使用しない場合
var students = new Student[8];
students[0] = new Student{ Id = 1, Name = "John Smith", Class = "A", Score = 70 };
students[1] = new Student{ Id = 2, Name = "Alice Johnson", Class = "B", Score = 85 };
students[2] = new Student{ Id = 3, Name = "Bob Brown", Class = "A", Score = 90 };
students[3] = new Student{ Id = 4, Name = "Charlie Davis", Class = "C", Score = 78 };
students[4] = new Student{ Id = 5, Name = "Diana Evans", Class = "B", Score = 92 };
students[5] = new Student{ Id = 6, Name = "Evan Harris", Class = "C", Score = 88 };
students[6] = new Student{ Id = 7, Name = "Fiona Green", Class = "A", Score = 75 };
students[7] = new Student{ Id = 8, Name = "George White", Class = "C", Score = 80 };

//コレクション初期化子を使用した場合
var students = new []
{
    new Student{ Id = 1, Name = "John Smith", Class = "A", Score = 70 },
    new Student{ Id = 2, Name = "Alice Johnson", Class = "B", Score = 85 },
    new Student{ Id = 3, Name = "Bob Brown", Class = "A", Score = 90 },
    new Student{ Id = 4, Name = "Charlie Davis", Class = "C", Score = 78 },
    new Student{ Id = 5, Name = "Diana Evans", Class = "B", Score = 92 },
    new Student{ Id = 6, Name = "Evan Harris", Class = "C", Score = 88 },
    new Student{ Id = 7, Name = "Fiona Green", Class = "A", Score = 75 },
    new Student{ Id = 8, Name = "George White", Class = "C", Score = 80 }
};
//配列の型名と要素の型名はどちらか一方に記述すればもう一方は省略できます。
var students = new Student[]
{
    new (){ Id = 1, Name = "John Smith", Class = "A", Score = 70 },
    new (){ Id = 2, Name = "Alice Johnson", Class = "B", Score = 85 },
    new (){ Id = 3, Name = "Bob Brown", Class = "A", Score = 90 },
    new (){ Id = 4, Name = "Charlie Davis", Class = "C", Score = 78 },
    new (){ Id = 5, Name = "Diana Evans", Class = "B", Score = 92 },
    new (){ Id = 6, Name = "Evan Harris", Class = "C", Score = 88 },
    new (){ Id = 7, Name = "Fiona Green", Class = "A", Score = 75 },
    new (){ Id = 8, Name = "George White", Class = "C", Score = 80 }
};
```

### 2.3 匿名クラス
匿名クラスは、クラス名を明示せずにオブジェクトを作成する機能です。これは一時的にデータをまとめる場合や、LINQのSelectメソッドなどでデータの型を変換する際に便利です。

```cs
//new{ } の中でプロパティ名と値を指定します
var query1 = students
  .Select(student => new {Class = student.Class, Name = student.Name});

//匿名クラスのプロパティ名と、代入元のオブジェクトのプロパティ名が一致している場合はプロパティ名が省力可能です・
var query2 = students
  .Select(student => new {student.Class, student.Name});

foreach(var student in students)
{
    Console.WriteLine($"{student.Class}: {student.Name}")
}

```

匿名クラスは、コンパイラによって内部で特定のクラスとして生成されます。この生成されたクラス名はコンパイラ内部でのみ利用され、プログラム内で直接利用することはできません。また、匿名クラスのプロパティは全て読み取り専用となり、値の書き換えは出来ません。
```cs
class <>f__AnonymousType0
{
    public string Class {get; }
    public string Name {get; }
}
```

### 2.4 拡張メソッド
拡張メソッドは、既存の型に新たなメソッドを追加する機能です。これにより、既存のクラスを継承または変更することなく、その型のインスタンスに対して新たなメソッドを定義することができます。
拡張メソッドは、静的クラス内の静的メソッドとして定義されます。このメソッドの第一引数には、thisキーワードと拡張する型の名前が指定されます。

```cs
static class StringExtensions
{
    public static bool IsNullOrEmpty(this string s)
    {
        return string.IsNullOrEmpty(s);
    }
}

//使用例
string test = null;
bool result = test.IsNullOrEmpty();  // true 
```

LINQ (to Objects)のクエリ操作は主に`IEmumerable<T>`の拡張メソッドとして定義されています。
拡張メソッドで実装することの利点は、VisualStudioなどのIDEが持コード補完機能（インテリセンス）と組み合わせた時に発揮されます。


## 3. LINQを構成するインターフェイス `IEnumerable<T>` と `IEnumerator<T>`

### `IEnumerable<T>`
`IEnumerable<T>`は、コレクションが反復処理可能なことを示すインターフェイスです。このインターフェイスを実装することで、その型のインスタンスをforeachループで反復処理することができます。

`IEnumerable<T>`インターフェイスは、`IEnumerator<T>`型のオブジェクトを返すGetEnumeratorメソッドを持ちます。

```cs
public interface IEnumerable<out T> : IEnumerable
{
    IEnumerator<T> GetEnumerator();
}
public interface IEnumerable
{
    IEnumerator GetEnumerator(); //非ジェネリック互換用
}
```

### `IEnumerator<T>`
`IEnumerator<T>`は、コレクションを一度に1つずつ反復処理するためのインターフェイスです。このインターフェイスは、CurrentプロパティとMoveNextメソッド、Resetメソッド及びDisposeメソッドを定義しています。
また、`IEnumerator<T>`を実装したオブジェクトの事を列挙子といいます。

- Current: 現在の要素を取得します。
- MoveNext: 次の要素に移動します。もし次の要素が存在すればtrueを返し、存在しなければfalseを返します。
- Reset: 列挙子を初期位置、つまりコレクションの最初の要素の前に戻します。  
  ※ 但し、Resetは実装が必須ではなく、実装されていないケースも多いため、基本的には利用を避けるべきです。
- Dispose: 反復処理の完了時に実行されます。列挙子が使用していたリソースの解放を行います。

```cs
public interface IEnumerator<out T> : IEnumerator, IDisposable
{
    T Current { get; }
}
public interface IEnumerator
{
    object Current { get; } //非ジェネリック互換用
    bool MoveNext();
    void Reset();
}
public interface IDisposable
{
    void Dispose();
}
```
## 4. 実習LINQ
ここからは、実際にコードを実装しながらLINQの仕組みや注意点について学んでいきたいと思います。

これ以降で紹介するコードは、本リポジトリ内にすべて含まれます。実際に動かし、デバッカでステップ実行するなどして理解を深めましょう。

### [sample-1] CountUpEnumerableとCountUpEnumerator
まずは、与えた初期値を１ずつカウントアップする列挙子`CountUpEnumerator`クラスとそれを提供する`CountUpEnumerable`クラスを作成します。

```cs
//値を1ずつカウントアップする列挙子
class CountUpEnumerator(int start) : IEnumerator<int>
{
    private int _value = start;
    public int Current { get; private set; }

    public bool MoveNext()
    {
        Current = _value;
        _value++;
        return true;
    }
    public void Dispose(){　}
    public void Reset() => throw new NotImplementedException();
    object IEnumerator.Current => Current;
}
//CountUp列挙子による列挙が可能なクラス
class CountUpEnumerable(int start) : IEnumerable<int>
{
    public IEnumerator<int> GetEnumerator()
    {
        return new CountUpEnumerator(start);
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
```

[LinqExplorer/Linq/Iterators/CountUpEnumerable.cs](./LinqExplorer/Linq/Iterators/CountUpEnumerable.cs)


クラスの作成ができたら、下記のサンプルコードを実行してみましょう。
`CountUpEnumerable`をインスタンス化し、foreachに渡して反復処理を実行します。

```cs
[Command("sample-1")]
public void Sample_1()
{
    ConsoleEx.WriteLine("# IEnumerable<T> と IEnumerator<T>", ConsoleColor.Magenta);
    foreach (var n in new CountUpEnumerable(1))
    {
        ConsoleEx.WriteLine(n, ConsoleColor.Green);
        if (n == 10) { break; }　//CountUpEnumeratorは永遠とカウントアップするため適度に中断
    }
}
```

foreachはコンパイル時に下記のようなコードに変換のうえ実行されます。
- GetEnumerator()で列挙子CountUpEnumeratorを取得します。
- MoveNext()を呼び出して次の要素に進み、その値をCurrentプロパティにセットします。  
  MoveNext()は列挙子が次に進むことができた場合にtrueを返します。列挙子が次に進むことができなかった場合、つまりコレクションの最後の要素を反復処理した後にMoveNextを呼び出した場合はfalseを返します。

```cs
//GetEnumerator()で列挙子を呼び出します
var enumerator = new CountUpEnumerable(1).GetEnumerator();
try
{
    //MoveNex()がtrueを返す間ループします
    while (enumerator.MoveNext())
    {
        //Currentプロパティから現在の値を取り出して表示します
        ConsoleEx.WriteLine(enumerator.Current, ConsoleColor.Green);
    }
}
finally
{
    //反復処理が完了したら、Dispose()を呼び出して列挙子が使用していたリソースを解放します
    enumerator.Dispose();
}
```

<details><summary>実行結果</summary>

```cmd
> dotnet run --project LinqExplorer sample-1
# IEnumerable<T> と IEnumerator<T>
@ GetEnumerator()
@ MoveNext() > Current=1
1
@ MoveNext() > Current=2
2
@ MoveNext() > Current=3
3
@ MoveNext() > Current=4
4
@ MoveNext() > Current=5
5
@ MoveNext() > Current=6
6
@ MoveNext() > Current=7
7
@ MoveNext() > Current=8
8
@ MoveNext() > Current=9
9
@ MoveNext() > Current=10
10
@ Dispose()
```

</details>

### [sample-2] イテレーター構文

`CountUpEnumerable`と`CountUpEnumerator`はイテレータ構文という機能を使って下記の様に書き換えることができます。

イテレータ構文は、`yield return`ステートメントを使用して値を順次返す機能です。この機能の特徴は、反復処理の現在位置を記憶しておくことができる点です。これにより、次に要素が必要になったときに、その記憶していた位置から処理を再開することが可能になります。

イテレータ構文は、IEnumerator<T>またはIEnumerable<T>を戻り値とするメソッド内で使用することができます。

- `IEnumerator<T>`を戻り値とするメソッドの例:
```cs
class CountUpEnumerable2(int start) : IEnumerable<int>
{
    public IEnumerator<int> GetEnumerator()
    {
        var value = start;
        while (true)
        {
            yield return value++;
        }
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
```

- `IEnumerable<T>`を戻り値とするメソッドの例:
```cs
static partial class MyEnumerable
{
    public static IEnumerable<int> CountUp(int start)
    {
        var value = start;
        while (true)
        {
            yield return value++;
        }
    }
}
```

[LinqExplorer/Linq/Iterators/CountUpEnumerable2.cs](./LinqExplorer/Linq/Iterators/CountUpEnumerable2.cs)

<details><summary>実行結果</summary>

```cmd
> dotnet run --project LinqExplorer sample-2
# イテレーター構文: IEnumerator<T>を戻り値とするパターン
@ CountUp(1)
@ CountUp(1): yield return > 1
1
@ CountUp(2): yield return > 2
2
@ CountUp(3): yield return > 3
3
@ CountUp(4): yield return > 4
4
@ CountUp(5): yield return > 5
5
@ CountUp(6): yield return > 6
6
@ CountUp(7): yield return > 7
7
@ CountUp(8): yield return > 8
8
@ CountUp(9): yield return > 9
9
@ CountUp(10): yield return > 10
10
# イテレーター構文: IEnumerable<T>を戻り値とするパターン
@ CountUp(1)
@ CountUp(1): yield return > 1
1
@ CountUp(2): yield return > 2
2
@ CountUp(3): yield return > 3
3
@ CountUp(4): yield return > 4
4
@ CountUp(5): yield return > 5
5
@ CountUp(6): yield return > 6
6
@ CountUp(7): yield return > 7
7
@ CountUp(8): yield return > 8
8
@ CountUp(9): yield return > 9
9
@ CountUp(10): yield return > 10
10
```

</details>

> [!NOTE]
> イテレータ構文で記述されたコードはコンパイル時、おおよそ下記のようなコードに変換されます。
> [LinqExplorer/Linq/Iterators/CountUpIterator.cs](./LinqExplorer/Linq/Iterators/CountUpIterator.cs)
> `IEnumerable<T>`と`IEnumerator<T>`を同時に実装したイテレータクラスが生成されて、状態管理などを行っています。
> イテレータ構文はこれらのコードを簡単に記述するための機能といえます。
> 
> .NETのライブラリでは、LINQの各クエリメソッドの内部で`IEnumerable<T>`と`IEnumerator<T>`を同時に実装した`Iterator<T>`というクラスが実装されています。どのような実装になっているかを実際のコードを見て確認してみるのも良いでしょう。
> https://github.com/dotnet/runtime/blob/main/src/libraries/System.Linq/src/System/Linq/Iterator.cs


### [sample-3] クエリメソッド(Where,Select,Take)の実装
イテレータ構文を使ってクエリメソッドを自作してみましょう。
MyEnumerableという静的クラスを作って、そこにSelect、Where、Takeと同等の機能を持つMySelect、MyWhere、MyTakeメソッドを実装します。

また、クエリ実行時の挙動を確認するためのコンソール出力も仕込んでおきます。
```cs
static class MyEnumerable
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
}
```

作成したクエリを下記コードで実行してみましょう

```cs
[Command("sample-3")]
public void Sample_3()
{
    Console.WriteLine("set query");
    var query = new CountUpIterator(1)
        .MyWhere(n => n % 2 == 0)
        .MySelect(n => n * n)
        .MyTake(5);

    Console.WriteLine(">>start foreach");
    foreach (var n in query)
    {
        ConsoleEx.WriteLine(n, ConsoleColor.Green);
    }
    Console.WriteLine("<< end foreach");
}
```

実行結果からforeach実行時の動作を確認してみましょう。

<details>
<summary>実行結果</summary>

```cmd
> dotnet run --project LinqExplorer sample-3
set query
>>start foreach
MyTake(5)
MySelect()
MyWhere()
@ GetEnumerator()
@ MoveNext() > Current=1
Where(1)
@ MoveNext() > Current=2
Where(2): yield return > 2
Select(2): yield return > 4
Take(4): yield return > 4
4
@ MoveNext() > Current=3
Where(3)
@ MoveNext() > Current=4
Where(4): yield return > 4
Select(4): yield return > 16
Take(16): yield return > 16
16
@ MoveNext() > Current=5
Where(5)
@ MoveNext() > Current=6
Where(6): yield return > 6
Select(6): yield return > 36
Take(36): yield return > 36
36
@ MoveNext() > Current=7
Where(7)
@ MoveNext() > Current=8
Where(8): yield return > 8
Select(8): yield return > 64
Take(64): yield return > 64
64
@ MoveNext() > Current=9
Where(9)
@ MoveNext() > Current=10
Where(10): yield return > 10
Select(10): yield return > 100
Take(100): yield return > 100
100
@ Dispose()
<< end foreach
```
</details>

#### 遅延実行
下記のコードが実行された時点ではLINQの列挙が行われていません。foreachが開始された時点で列挙が始まり、各クエリメソッドが動き出すのが分かります。
```cs
//ここではクエリの手順が記録されるだけ
var query = new CountUpIterator(1)
    .MyWhere(n => n % 2 == 0)
    .MySelect(n => n * n)
    .MyTake(5);
```

#### メソッドチェーンの挙動
実行結果を見ると、下記のような流れで処理が進んでいることが分かります。
`yield return` した時点で処理が呼び出し元に移り、メソッドチェーンを辿る様子がイメージできたのではないでしょうか。

![sequence.svg](/image/sequence.svg)

### [sample-4] クエリメソッド(Count,ToList)の実装

#### 遅延実行と即時実行
sample-3 ではクエリメソッドが遅延実行されることを確認しましたが、関数を記述した位置で即座に実行（即時実行）されるクエリも存在します。

どのクエリメソッドがどちらのタイプに属するかは、下記リンクの分類表で確認できます。
[C# での LINQ クエリの概要#分類表:Microsoft Learn](
https://learn.microsoft.com/ja-jp/dotnet/csharp/linq/get-started/introduction-to-linq-queries#classification-table)

このドキュメントでは、クエリメソッドは下記の3タイプに分類されています。各タイプの詳細はドキュメントの説明を確認してください。
- 即時実行:  主にスカラ値(単一の値)を返すクエリ`Count`,`Max`,`Average`,`First`など
- 遅延実行(ストリーミング): 主に`IEnumerable<T>`を返すクエリ `Where`,`Select`,`Skip`など
- 遅延実行(非ストリーミング): 主に並び替えやグループ化などを行うクエリ`GrpupBy`,`OrderBy`など

#### 即時実行するクエリの実装
Count,ToListを例に即時実行するクエリメソッドの動きを確認してみましょう。
即時実行するクエリメソッドは`yield return`を使わず、通常のforeachを最後までまわします。クエリメソッド自体がforeachの列挙を実行する必要があるため、即時実行になるわけです。

```cs
static class MyEnumerable
{
    public static int MyCount<T>(this IEnumerable<T> source)
    {
        Console.WriteLine($"MyCount()");
        var count = 0;
        foreach (var item in source)
        {
            count++;
            ConsoleEx.WriteLine($"Count({item}): count = {count}", ConsoleColor.Green);
        }
        return count; //全要素をカウントした後、最後に結果を返す
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
        return list; //全要素をListに詰めた後にそのリストを返す
    }
}
```

下記コードで実行してみましょう。

```cs
[Command("sample-4")]
public void Sample_4()
{
    ConsoleEx.WriteLine("# Count() と ToList()", ConsoleColor.Magenta);
    var query = new CountUpIterator(1)
        .MyWhere(n => n % 2 == 0)
        .MySelect(n => n * n)
        .MyTake(5);

    Console.WriteLine("Count()");
    var count = query.MyCount();
    ConsoleEx.WriteLine($"Count={count}", ConsoleColor.Green);

    ConsoleEx.WriteLine("-----------------------------", ConsoleColor.Magenta);
    Console.WriteLine("ToList()");
    var list = query.MyToList();
    ConsoleEx.WriteLine($"[{string.Join(",", list)}]", ConsoleColor.Green);
}
```

<details><summary>実行結果</summary>

```cmd
> dotnet run --project LinqExplorer sample-4
# Count() と ToList()
@ new CountUpIterator()
Count()
MyCount()
MyTake(5)
MySelect()
MyWhere()
@ GetEnumerator()
@ MoveNext() > 1
Where(1)
@ MoveNext() > 2
Where(2): yield return > 2
Select(2): yield return > 4
Take(4): yield return > 4
Count(4): count = 1
@ MoveNext() > 3
Where(3)
@ MoveNext() > 4
Where(4): yield return > 4
Select(4): yield return > 16
Take(16): yield return > 16
Count(16): count = 2
@ MoveNext() > 5
Where(5)
@ MoveNext() > 6
Where(6): yield return > 6
Select(6): yield return > 36
Take(36): yield return > 36
Count(36): count = 3
@ MoveNext() > 7
Where(7)
@ MoveNext() > 8
Where(8): yield return > 8
Select(8): yield return > 64
Take(64): yield return > 64
Count(64): count = 4
@ MoveNext() > 9
Where(9)
@ MoveNext() > 10
Where(10): yield return > 10
Select(10): yield return > 100
Take(100): yield return > 100
Count(100): count = 5
@ Dispose()
Count=5
-----------------------------
ToList()
MyToList()
MyTake(5)
MySelect()
MyWhere()
@ GetEnumerator()
@ new CountUpIterator()
@ MoveNext() > 1
Where(1)
@ MoveNext() > 2
Where(2): yield return > 2
Select(2): yield return > 4
Take(4): yield return > 4
ToList(4)
@ MoveNext() > 3
Where(3)
@ MoveNext() > 4
Where(4): yield return > 4
Select(4): yield return > 16
Take(16): yield return > 16
ToList(16)
@ MoveNext() > 5
Where(5)
@ MoveNext() > 6
Where(6): yield return > 6
Select(6): yield return > 36
Take(36): yield return > 36
ToList(36)
@ MoveNext() > 7
Where(7)
@ MoveNext() > 8
Where(8): yield return > 8
Select(8): yield return > 64
Take(64): yield return > 64
ToList(64)
@ MoveNext() > 9
Where(9)
@ MoveNext() > 10
Where(10): yield return > 10
Select(10): yield return > 100
Take(100): yield return > 100
ToList(100)
@ Dispose()
[4,16,36,64,100]
```

</details>

全体の動きは遅延実行の場合とほとんど変わっていません。Count、ToListがforeachと置き換わっているのが分かります。

### [sample-5] クエリ結果をメモリに格納する
即時実行するクエリメソッドの中でも下記メソッドは、クエリの実行結果をメモリに保存する際に利用します。

- ToArray
- ToList
- ToDictionary
- ToLookUp
- 
同じクエリを複数回foreachで処理するような場合、これらを使ってクエリを即時実行し、その結果を利用した方がパフォーマンス的に有利になる場合があります。

[C# での LINQ クエリの概要:クエリ結果をメモリに格納する](https://learn.microsoft.com/ja-jp/dotnet/csharp/linq/get-started/introduction-to-linq-queries#store-the-results-of-a-query-in-memory)

次のプログラムを実行してみましょう。

```cs
[Command("sample-5")]
public void Sample_5()
{
    ConsoleEx.WriteLine("# ToList()によるクエリ結果のキャッシュ", ConsoleColor.Magenta);
    var list = new CountUpIterator(1)
        .MyWhere(n => n % 2 == 0)
        .MySelect(n => n * n)
        .MyTake(5)
        .MyToList();
    　
    ConsoleEx.WriteLine($"Count= {list.Count} //List<T>のCountプロパティから取得出来る", ConsoleColor.Green);

    ConsoleEx.WriteLine("# ここでforeachしてもクエリの再評価は行われない", ConsoleColor.Magenta);
    Console.WriteLine(">>start foreach");
    foreach (var item in list)
    {
        ConsoleEx.WriteLine(item, ConsoleColor.Green);            
    }
    Console.WriteLine("<<end foreach");
}
```
<details><summary>実行結果</summary>

```cmd
> dotnet run --project LinqExplorer sample-5
# ToList()によるクエリ結果のキャッシュ
@ new CountUpIterator()
MyToList()
MyTake(5)
MySelect()
MyWhere()
@ GetEnumerator()
@ MoveNext() > 1
Where(1)
@ MoveNext() > 2
Where(2): yield return > 2
Select(2): yield return > 4
Take(4): yield return > 4
ToList(4)
@ MoveNext() > 3
Where(3)
@ MoveNext() > 4
Where(4): yield return > 4
Select(4): yield return > 16
Take(16): yield return > 16
ToList(16)
@ MoveNext() > 5
Where(5)
@ MoveNext() > 6
Where(6): yield return > 6
Select(6): yield return > 36
Take(36): yield return > 36
ToList(36)
@ MoveNext() > 7
Where(7)
@ MoveNext() > 8
Where(8): yield return > 8
Select(8): yield return > 64
Take(64): yield return > 64
ToList(64)
@ MoveNext() > 9
Where(9)
@ MoveNext() > 10
Where(10): yield return > 10
Select(10): yield return > 100
Take(100): yield return > 100
ToList(100)
@ Dispose()
Count= 5 //List<T>のCountプロパティから取得出来る
# ここでforeachしてもクエリの再評価は行われない
>>start foreach
4
16
36
64
100
<<end foreach
```

</details>

`ToList()`でクエリが即時実行され、その結果がListに保存されたことで、次のforeachではクエリの再評価が行われない事が分かります。
また、要素数（Count）は、処理結果の`List<T>`のCountプロパティを使えば、Count()メソッドによるクエリの再実行は不要となります。