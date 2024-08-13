# LINQ勉強会資料

## 1. LINQとは
LINQ (Language Integrated Query) は、.NETフレームワークに組み込まれた強力なデータクエリ機能です。

LINQの主な特徴は以下の通りです：
- 統一的な構文：LINQは、配列、リスト、XML、データベースなど、さまざまなデータソースに対して同じクエリ構文を使用できます。
- 強い型付け：LINQクエリはコンパイル時に型チェックされるため、型ミスなどのエラーを早期に検出できます。
- IntelliSense サポート：Visual StudioのIntelliSenseはLINQクエリをサポートしており、クエリの作成を容易にします。

### 1.1 LINQプロバイダー
LINQを使用すると、データベース、XML、コレクションなど、さまざまなデータソースに対して一貫した方法でクエリを実行することが可能です。以下に、いくつかの主要なLINQプロバイダーを示します。

- LINQ to SQL
- LINQ to XML 
- LINQ to JSON
- LINQ to Objects

LINQ to Objectsは、C#のコレクション（配列やListなど）に対してクエリを実行します。このテキストでは、主にLINQ to Objectsについて解説します。


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

https://learn.microsoft.com/ja-jp/dotnet/csharp/linq/get-started/features-that-support-linq

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
匿名メソッド式は、必要な場所で直接メソッドを定義するための機能です。
これにより、一度しか使用しないメソッドを作成する際に、メソッド名を付ける必要がなくなり、コードが簡潔になります。

```cs
class A
{
    public void Sample()
    {
        IntToString intToString = delegate(int n)
        { 
            return n.ToString
        };
    }
}
```

#### ラムダ式
ラムダ式は、匿名メソッド式から省略可能な部分を省き、さらに簡略化したものです。
関数を引数として渡す際や、一度しか使用しない関数を定義する際に有用です。

```cs
class A
{
    public void Sample()
    {
        //delegeteキーワードを省略
        //引数の型が推論できる場合は省略可
        //引数が1つの場合、()の省略が可能
        //メソッドの本体が単一ステートメント（1行で書ける）の場合は {} と return　が省略可能
        IntToString intToString = n => n.ToString;
    }
}
```

### 2.2 オブジェクト初期化子とコレクション初期化子
オブジェクト初期化子を利用すると、単一ステートメントでインスタンスの生成とプロパティの初期化が同時に行えます。

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
//単一のステートメントでインスタンスの生成とプロパティの初期化が同時に行えます。
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
dotnet run --project LinqExplorer sample-1
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
C:\Work\LinqExplorer> dotnet run --project LinqExplorer sample-2
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
@ CountUp(1):return > 1
1
@ CountUp(2):return > 2
2
@ CountUp(3):return > 3
3
@ CountUp(4):return > 4
4
@ CountUp(5):return > 5
5
@ CountUp(6):return > 6
6
@ CountUp(7):return > 7
7
@ CountUp(8):return > 8
8
@ CountUp(9):return > 9
9
@ CountUp(10):return > 10
10
```

</details>

> [!NOTE]
> イテレータ構文で記述されたコードはコンパイル時、下記のようなコードに変換されます。
> [LinqExplorer/Linq/Iterators/CountUpIterator.cs](./LinqExplorer/Linq/Iterators/CountUpIterator.cs)
> `IEnumerable<T>`と`IEnumerator<T>`を同時に実装したイテレータクラスが生成されて、状態管理などが行っています。
> イテレータ構文はこれらのコードを簡単に記述するための機能といえるでしょう。
> 
> 実際.NETのライブラリでは、LINQの各クエリメソッドの内部で`IEnumerable<T>`と`IEnumerator<T>`を同時に実装した`Iterator<T>`というクラスが実装されており、下記より確認することができます。
> https://github.com/dotnet/runtime/blob/main/src/libraries/System.Linq/src/System/Linq/Iterator.cs

