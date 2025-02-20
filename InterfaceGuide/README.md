# Interface
インターフェースは、複雑なシステムやプロセスを抽象化し、ユーザーがその本質的な部分に集中できるようにするための手段です。

### 抽象化
抽象化するとは、具体的な事例や詳細から共通する特徴や本質を取り出し、一般的な概念やモデルにまとめることを指します。これにより、複雑な情報をシンプルにし、理解しやすくすることができます。

- 複雑さの軽減: 抽象化により、複雑なシステムや問題をよりシンプルに理解しやすくなります。
- 再利用性の向上: 抽象化された概念やモデルは、異なる状況やプロジェクトで再利用しやすくなります。
- 柔軟性の向上: 抽象化により、システムやプロセスの変更が容易になります。具体的な実装に依存しないため、変更が必要な場合でも影響を最小限に抑えることができます。
- コミュニケーションの改善: 抽象化された概念は、異なる専門分野や背景を持つ人々とのコミュニケーションを円滑にします。共通の理解を持ちやすくなるため、協力がしやすくなります。
- 問題解決の効率化: 抽象化により、問題の本質を見極めやすくなり、効果的な解決策を見つけやすくなります。
 
## オブジェクト指向プログラミングにおけるインターフェース
オブジェクト指向プログラミングの世界においては、 インターフェースとはクラスが実装すべき規約（どういうメソッドにどういう引数を渡すかなど）を定めるものです。
すなわち、クラス設計者とクラス利用者の間の仲介役を担うのがインターフェースです。

インターフェースを規定するもの
- 引数の数と型：インプット
- 戻り値の型：アウトプット
- インターフェース、メソッドの名前：持つべき機能

公開するメソッドとその規約だけを定めて、具体的な実装は継承先に任せる

> [!NOTE]
> - インターフェース: クラス外部からみた規約だけを定めるもの。「クラスの内外の境界」という意味。
> - public な抽象メソッドだけを持つクラスのようなもの。
> - C# 8.0 で緩和されて、「フィールドを持てない代わりに多重継承できる」くらいの差に縮まっています
> - 抽象クラスと違って、複数のインターフェースを継承できる。
> - class キーワードの代わりに interface キーワードを使う。
> 
> https://ufcpp.net/study/csharp/oo_interface.html

## インターフェースの役割

1. 持っている機能を明確にする。規約に従った機能を持つことを保証する。  
   利用者に対してカタログスペックを公開する。〇〇出来る、〇〇の機能を持つ

    - IEnumerable: 列挙可能
    - IEquatable: 等価性の判定ができる
	- IFormattable: フォーマット可能
 
2. 使える機能を限定する
   必要のない操作を隠ぺいして、使ってほしい操作のみを公開する。（安全性）  
   利用可能な操作のみを公開することで、ユーザが迷いなく使える。（利便性）

    - IReadOnlyCollection: コレクションの読み取りのみ（書き込み操作は持たない）
    - IEnumerable: 列挙"のみ"可能 (インデックスによるアクセスや、要素数の取得もできない)

```cs
public class A
{
    private IList<string> items;

    public IReadOnlyCollection<string> Items { get => items; }
}
```

```cs
var a = new A();
//IReadOnlyCollection<T>型のItemsプロパティは
//CountプロパティとGetEnumerator()メソッド(=foreach)のみが利用可能
var count = a.Items.Count;
foreach(var item in a.Items)
{
　　　Console.WriteLine(item);
}
```

    
3. 問題の分離と単純化
　複雑な問題を分割・単純化し、扱うべき問題の影響範囲をコントロールする。  
　※今回取り上げるのはこの部分


### SOLIDの原則

- 単一責任の原則 (single-responsibility principle)  
  - 変更するための理由が、一つのクラスに対して一つ以上あってはならない
  - 各クラスはそれぞれ一つだけの責務を持つべきである
- 開放閉鎖の原則（open/closed principle）  
  - ソフトウェアの実体（クラス、モジュール、関数など）は、拡張に対して開かれているべきであり、修正に対して閉じていなければならない
- リスコフの置換原則（Liskov substitution principle）
  - ある基底クラスへのポインタないし参照を扱っている関数群は、その派生クラスのオブジェクトの詳細を知らなくても扱えるようにしなければならない 
- インターフェース分離の原則 (interface segregation principle)
  - 汎用なインターフェースが一つあるよりも、各クライアントに特化したインターフェースがたくさんあった方がよい
- 依存性逆転の原則（dependency inversion principle）
  - 上位モジュールはいかなるものも下位モジュールから持ち込んではならない。
  - 双方とも具象ではなく、抽象（インターフェースなど）に依存するべきである

https://ja.wikipedia.org/wiki/SOLID


## 演習
以下では、インターフェースの使い方について実際にソースコードを見ながら学びたいと思います。
インターフェースを一切利用しないLevel1から段階的にリファクタリングを行い、インターフェースの活用例とその利点を体感しましょう。

### 題材
今回の例では、図形（四角形や楕円など）を描画するプログラムを考えます。
以下の記事で紹介しているプログラムが元ネタですが、この教材での説明用に単純化し、再構成しています。
https://qiita.com/pierusan2010/items/9cd5af84d70fe345c968

### Level1

[ソースコード:Level1](/InterfaceGuide/Level1.cs)

最初のプログラムは、以下のような構成になっています
- 図形を表すShapeクラスがあります
- Shapeクラスには下記の機能があります
  - Draw(): 図形を描画する
  - Containts(x,y): 指定した点が図形の領域内にあるかを判定する。領域内ならTrueを返す
- 図形のタイプ(四角形、楕円)はenum `ShapeType` で指定します
- 図形の描画には`WinDrawing`という（架空の）2Dグラフィック用のライブラリが提供する`WinGraphics`というクラスを利用します。  
  (WinDrawingはWindowsでのグラフィックに特化したWindows環境でのみ動作するライブラリを想定しています)

![level1](/image/ShapeL1.png)

#### 課題1

Level1のプログラムでは、異なる図形をShapeクラス１つで賄っているため、図形タイプ毎に処理を振り分ける分岐（switch文）が必要となります。
この実装では、新しく機能を追加したいときに下記のような問題が発生します。
- 新たに図形のタイプを追加したい場合、分岐(switch)のある個所を全て変更しなければならない
- Draw(),Containts()の他に新しい機能を追加した場合にも新たな分岐処理が追加される恐れがある。

> [!NOTE]
> 同じ条件のswitch文が2つ以上出てきたら要注意！switch文をなくす、または1箇所に絞れないかを考えよう

```cs
public void Draw()
{
    switch (Type)
    {
        case ShapeType.Rectangle:
            _g.DrawRect(Bounds.ToWRect());
            break;
        case ShapeType.Oval:
            _g.DrawOval(Bounds.ToWRect());
            break;
    }
}
public bool Contains(double X, double Y)
{
    switch (Type)
    {
        case ShapeType.Rectangle:
            return Bounds.X < X && X < Bounds.X + Bounds.Width
                && Bounds.Y < Y && Y < Bounds.Y + Bounds.Height;
        case ShapeType.Oval:
            var ar = Bounds.Width / 2;
            var br = Bounds.Height / 2;
            var px = X - Bounds.X - ar;
            var py = X - Bounds.Y - br;
            return (px * px) / (ar * ar) + (py * py) / (br * br) < 1;
        default:
            return false;
    }
}
```
### Level2
問題点1を解決するためにはどうすればよいでしょう。

1. まずはShapeクラスのが提供する"外側から見た"機能をインターフェースとして抜き出します(抽象化)

```cs
interface IShape
{
    void Draw();
    bool Contains(double x, double y);
}
```

2. 次に各種図形をこのインターフェースを継承したクラスとして実装します。  
   メソッド内の処理は、図形クラスごとに自分の処理だけを記述すればよく、switch文による分岐処理が不要になります。

```cs
class RectangleShape(Rectangle bounds) : IShape
{
    private WinGraphics _g = new();
    public Rectangle Bounds { get; } = bounds;
    public void Draw()
    {
        _g.DrawRect(Bounds.ToWRect());
    }

    public bool Contains(double X, double Y)
    {
        return Bounds.X < X && X < Bounds.X + Bounds.Width
            && Bounds.Y < Y && Y < Bounds.Y + Bounds.Height;
    }
}

class OvalShape(Rectangle bounds) : IShape
{
    private WinGraphics _g = new();
    public Rectangle Bounds { get; } = bounds;
    public void Draw()
    {
        _g.DrawOval(Bounds.ToWRect());
    }
    public bool Contains(double X, double Y)
    {
        var ar = Bounds.Width / 2;
        var br = Bounds.Height / 2;
        return (Bounds.X * Bounds.X) / (ar * ar) + (Bounds.X * Bounds.X) / (br * br) < 1;
    }
}
```

3. 図形クラスを使う側は、全て抽象化された`IShape`として扱います。
   そうすることで、今どの図形を扱っているかを気にせずに利用することができます。

```cs
//図形の種類を気にするのは、オブジェクト生成時のみ
var shapes = new IShape[] {
    new RectangleShape(new Rectangle(100, 100, 150, 150)),
    new OvalShape(new Rectangle(300, 100, 120, 180)),
};

var cursorX = 123;
var cursorY = 144;
//抽象化してしまえば、異なるクラス(図形)でも同じものとして扱える
foreach (var shape in shapes)
{
    shape.Draw();
    var contains = shape.Contains(123, 123);
    Console.WriteLine($"Contains({cursorX},{cursorY}): {contains}");
}
```



![level2](/image/ShapeL2.png)

[ソースコード:Level2](/InterfaceGuide/Level2.cs)


### 課題2
このライブラリはWindowsのみをターゲットとしていましたが、Linux上でも動作するように対応したいと考えています。
現在図形の描画に使っているライブラリ`WinDrawing`はWindows上でのみ動作します。
Windows環境では現在の`WinDrawing`を利用しますが、Linux環境ではLinux上で動作する図形描画ライブラリ（`XDrawing`）を利用するように切り替え可能としたいと思います。


WinDrawingのWinGraphicsクラスは図形クラスの中でインスタンスの生成が行われています。図形クラスがWinGraphicsクラスに直接依存している状態です。
このままでは、簡単に切り替え出来そうにありません。

### Level3
図形描画ライブラリ`WinDrawing`と`XDrawing`のどちらを利用するかは、図形クラス（`IShape`を実装したクラス）を使う側でのみ判断すべきです。
図形クラス内で図形描画を行うクラス（`WinGraphics`）を直接生成するのではなく、必要な時に外部から受け取るようにすると上手くいきそうです。

1. まず図形描画クラスが持つ、このアプリケーションに必要な機能を抽出します⇒抽象化

```cs
interface IGraphics
{
    void DrawRectangle(Rectangle bounds);
    void DrawOval(Rectangle bounds);
}
```

1. `IGraphics`の具体的な実装は利用する側に任せます。

```cs
class WinGraphicsWrapper : IGraphics
{
    private readonly WinGraphics g = new();

    public void DrawOval(Rectangle bounds)
    {
        g.DrawOval(bounds.ToWRect());
    }

    public void DrawRectangle(Rectangle bounds)
    {
        g.DrawRect(bounds.ToWRect());
    }
}
```


3. このインターフェースを実装したオブジェクトを受け取る口を用意します ⇒依存性の注入
   受け渡し方はいくつかあります。
   - コンストラクタの引数として渡す
   - プロパティに設定する
   - メソッドの引数として渡す
  
   どれを使うかはケースバイケースですが、今回のケースでは`IShape`の`Draw`メソッドの引数として渡すようにします。

```cs
interface IShape
{
    void Draw(IGraphics g);
    bool Contains(double x, double y);
}
```

こうして実装した結果をクラス図で見てみましょう

![level3](/image/ShapeL3.png)

- 図形クラスを提供するライブラリ（Shapes3）から外部への依存が一切なくなりました。
  - ⇒依存性逆転の原則: 抽象（`IGraphics`）に依存することで、Shapes3への依存のみに変わった
- 新しい図形描画モジュールに対応する場合でも、Shapes3側に手を加えることなく新しい処理を追加できます。  
  - ⇒開放閉鎖の原則: Shapes3は修正に対して閉じており、拡張に対しては開いている

もし、Unitテスト用に画像描画モジュールのモックを追加したくなった場合でも、`IGraphics`を継承したモッククラスを用意するだけで済みます。


[ソースコード:Level3](/InterfaceGuide/Level3.cs)


### 課題3
指定した点が図形の内部か否かを判定する処理（`Contains`メソッド）の処理方式を選べるようにしたい。
- 図形の方程式から自前で計算
- 図形描画ライブラリが持つRegionを用いた判定

### Level4
上記のようにアプリケーションの処理方式（アルゴリズム）を動的に切り替えたい場合の一般的な設計パターンとして、[ストラテジーパターン](https://ja.wikipedia.org/wiki/Strategy_%E3%83%91%E3%82%BF%E3%83%BC%E3%83%B3)があります。

処理方式を切り替え可能とする、という意味ではLevel3の実装と同じです。クラスの構成などもほぼ変わりはありません。



![level4](/image/ShapeL4.png)

[ソースコード:Level3](/InterfaceGuide/Level4.cs)
