# マルチスレッドプログラミング

## 1. スレッドとは
プログラムの中で並行して実行される独立した一連の命令のこと

- 複数の処理を同時に実行することができる
- UIをブロックせずに、バックグラウンドで処理を実行できる
- メモリ等のリソースはスレッド間で共有される

> [!NOTE]
> ### プロセスとスレッド
> スレッドと同じようにプログラムの並行実行に関する概念にプロセスというものがあります。
> プロセスとスレッドには下記のような違いがあります。
>
>| 特徴       | スレッド (Thread)                             | プロセス (Process)                                |
>|------------|---------------------------------------------|---------------------------------------------|
>| 定義       | プロセス内で実行される独立した一連の命令       | 実行中のプログラムのインスタンス               |
>| メモリ共有 | 同じプロセス内でメモリを共有                  | 各プロセスは独立したメモリ空間を持つ            |
>| 作成のコスト| 低い（軽量）                                | 高い（重量）                                  |
>| 実行効率   | 高い（スレッド間の切り替えが高速）            | 低い（プロセス間の切り替えが遅い）              |
>| 安全性     | 共有メモリによるデータ競合のリスクがある      | 独立したメモリ空間による高い隔離性               |
>| 通信手段   | 共有メモリを使用                             | IPC（プロセス間通信）を使用（パイプ、ソケットなど） |

## 2. スレッドの管理

### Threadクラス
C#では`System.Threading`名前空間にスレッドを扱うための基本クラス`Thread`が定義されています。

```csharp
using System.Threading;

Thread myThread = new Thread(MyThreadMethod);
myThread.Start();

void MyThreadMethod()
{
    // スレッドで実行する処理
}
```
### ThreadPool
スレッドは生成のためのコストが高く、生成／破棄を頻繁に繰り返すとパフォーマンスに影響が出ます。
それを解消するための仕組みがスレッドプールです。予め生成しておいた複数のスレッドを効率よく使いまわす仕組みが備わっています。


```csharp
using System;
using System.Threading;

class Program
{
    static void Main()
    {
        // ThreadPoolを使って新しいスレッドをキューに追加
        ThreadPool.QueueUserWorkItem(new WaitCallback(MyThreadPoolMethod), "Task 1");
        ThreadPool.QueueUserWorkItem(new WaitCallback(MyThreadPoolMethod), "Task 2");
        ThreadPool.QueueUserWorkItem(new WaitCallback(MyThreadPoolMethod), "Task 3");

        // 主スレッドの待機
        Console.WriteLine("主スレッドが完了するまで待機中...");
        Thread.Sleep(3000); // 3秒待機（タスクが完了するのを待つ）

        Console.WriteLine("主スレッドが完了しました。");
    }

    static void MyThreadPoolMethod(object state)
    {
        string taskName = (string)state;
        Console.WriteLine($"{taskName}がThreadPoolで実行されています。");

        // タスクのシミュレーション（1秒待機）
        Thread.Sleep(1000);

        Console.WriteLine($"{taskName}が完了しました。");
    }
}
```

### スレッドの同期（Monitor/Lock）
同じプロセス内のスレッドは、メモリ空間を共有しているため、同時にアクセスした場合にデータの競合が発生する恐れがあります。それを回避するための機構は Monitorとlockです。

Monitorの例
```csharp
private static readonly object _lockObj = new object();
private static int _sharedResource = 0;

public void Increment()
{
    Monitor.Enter(_lockObj);
    try
    {
        //このブロック内のコードは1つのスレッドしか同時に実行できません（クリティカルセクション）
        _sharedResource++;
        Console.WriteLine("Shared Resource: " + _sharedResource);
    }
    finally
    {
        Monitor.Exit(_lockObj);
    }
}

```
 
lockの例：lock文はMonitorの簡易版
```csharp
private static readonly object _lockObj = new object();
private static int _sharedResource = 0;

public void Increment()
{
    lock (_lockObj)
    {
        //このブロック内のコードは1つのスレッドしか同時に実行できません（クリティカルセクション）
        _sharedResource++;
        Console.WriteLine("Shared Resource: " + _sharedResource);
    }
}

```

#### lock、Monitorの注意点
- デッドロック：ロックを取得した後に他のリソースのロックを待つとデッドロックが発生する可能性があるため、注意が必要
- パフォーマンスの問題：過剰なロックはパフォーマンスの低下を招く可能性がある


## 3. 並列処理と並行処理
並列処理と並行処理は、どちらも複数のタスクを同時に実行するための方法ですが、少し異なる概念です。

- **並列処理（Parallelism）**:
  - 同時に複数のタスクを実行する。
  - 複数のCPUコアを利用して、物理的に並列にタスクを処理。
  - 例えば、画像処理のようなCPUバウンドタスク。

- **並行処理（Concurrency）**:
  - 複数のタスクが交互に進行する。
  - 実際にはシングルコアであっても、タスクが細切れに実行されて並行しているように見える。
  - 例えば、サーバーでの複数のクライアントリクエストの処理。

### CPUバインドとIOバインド
同時に実行するタスクには、大きく分けて2つの特性があります。

- **CPUバインド（CPU-bound）**:
  - プロセッサの計算能力を最大限に活用するタスク。
  - 並列処理が効果的。
  - 例: 数値計算、データ解析、暗号化処理。

- **IOバインド（I/O-bound）**:
  - 入出力操作が主なボトルネックとなるタスク。
  - 並行処理が効果的。
  - 例: データベースアクセス、ファイル読み書き、ネットワーク通信。

### TaskとParallel

C#では、TaskとParallelクラスを使って非同期処理や並列処理を行うことができます。

- **Task**:
  - 非同期操作を簡単に扱うためのクラス。
  - `async`と`await`キーワードを使用して非同期処理を行う。

- **Parallel**:
  - 並列処理を簡単に行うためのクラス。
  - `Parallel.For`や`Parallel.ForEach`を使用して、ループ内の処理を並列化。
  - 例:
    ```csharp
    Parallel.For(0, 100, i =>
    {
        // ここに並列で実行したいコードを書く
        Console.WriteLine(i);
    });
    ```

## 4. C#でのマルチスレッドプログラミング
C#でマルチスレッドプログラミングをサポートする仕組み

### Delegateを利用した非同期処理
- 導入時期: .NET Framework 1.0
  
デリゲートを使用して非同期処理を実装することができます。BeginInvokeメソッドを使用して非同期にメソッドを呼び出し、EndInvokeメソッドで結果を取得します。

#### IAsyncResultパターン
非同期処理のための古い方法の一つで、主に .NET Frameworkの初期バージョンで使用されました。非同期メソッドは `Begin` で始まり `End` で終わる命名規則に従います。

```csharp
delegate int MyDelegate(int x);

MyDelegate del = new MyDelegate(MyMethod);
IAsyncResult asyncResult = del.BeginInvoke(10, null, null);
int result = del.EndInvoke(asyncResult);

int MyMethod(int x)
{
    return x * x;
}
```

### BackgroundWorker
- 導入時期: .NET Framework 2.0
  
BackgroundWorkerクラスは、バックグラウンドで処理を実行するための簡便なクラスです。進行状況の報告やキャンセル機能もサポートしています。主にWindowsFormsやWPFなどGUIアプリケーションで利用されます。

```csharp
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

public class ExampleForm : Form
{
    private BackgroundWorker backgroundWorker;
    private ProgressBar progressBar;
    private Button startButton;
    private Button cancelButton;
    private Label progressLabel;

    public ExampleForm()
    {
        // コントロールの初期化
        progressBar = new ProgressBar() { Location = new System.Drawing.Point(20, 20), Width = 260 };
        startButton = new Button() { Location = new System.Drawing.Point(20, 60), Text = "開始" };
        cancelButton = new Button() { Location = new System.Drawing.Point(100, 60), Text = "キャンセル" };
        progressLabel = new Label() { Location = new System.Drawing.Point(20, 100), Width = 260 };

        // コントロールをフォームに追加
        Controls.Add(progressBar);
        Controls.Add(startButton);
        Controls.Add(cancelButton);
        Controls.Add(progressLabel);

        // BackgroundWorkerの初期化
        backgroundWorker = new BackgroundWorker();
        backgroundWorker.WorkerReportsProgress = true;
        backgroundWorker.WorkerSupportsCancellation = true;
        backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
        backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
        backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);

        // ボタンのイベントハンドラを設定
        startButton.Click += new EventHandler(startButton_Click);
        cancelButton.Click += new EventHandler(cancelButton_Click);
    }

    private void startButton_Click(object sender, EventArgs e)
    {
        if (!backgroundWorker.IsBusy)
        {
            backgroundWorker.RunWorkerAsync();
            progressLabel.Text = "処理中...";
        }
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
        if (backgroundWorker.IsBusy)
        {
            backgroundWorker.CancelAsync();
        }
    }

    private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
        BackgroundWorker worker = sender as BackgroundWorker;

        for (int i = 0; i <= 100; i++)
        {
            if (worker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            // 1秒待機（シミュレーション）
            Thread.Sleep(100);
            worker.ReportProgress(i);
        }
    }

    private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        progressBar.Value = e.ProgressPercentage;
        progressLabel.Text = $"進捗: {e.ProgressPercentage}%";
    }

    private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        if (e.Cancelled)
        {
            progressLabel.Text = "キャンセルされました。";
        }
        else if (e.Error != null)
        {
            progressLabel.Text = $"エラーが発生しました: {e.Error.Message}";
        }
        else
        {
            progressLabel.Text = "処理が完了しました。";
        }
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new ExampleForm());
    }
}
```

### Task
- 導入時期:   
  .NET Framework 4.0 (Task)  
  .NET Framework 4.5　(C#5.0) (async/await)  

タスクベースの非同期パターン (TAP) 。`Task` クラスと `async`/`await` キーワードを使って非同期プログラミングを簡単に実装できます。

```csharp
async Task MyTask()
{
    await Task.Run(() =>
    {
        // 非同期処理
    });
}

await MyTask();

```

### ReactiveExtensions
Reactive Extensions（Rx）は、非同期およびイベントベースのプログラムを作成するためのライブラリで、特に観察可能なシーケンス（Observable Streams）を活用します。Rxは、時間と共に変化するデータストリームを簡単に操作するための一連の操作（フィルター、変換、集約など）を提供します。

```csharp
// Observableを作成し、HTTPリクエストを非同期に行う
var observable = Observable.FromAsync(() => {
    using HttpClient client = new HttpClient();
    HttpResponseMessage response = await client.GetAsync("https://consoto.com/api/333");
    response.EnsureSuccessStatusCode();
    return await response.Content.ReadAsStringAsync();
});

// Observableを購読してコンソールに出力
var subscription = observable.Subscribe(
    data => Console.WriteLine($"Received data: {data}"),
    error => Console.WriteLine($"Error occurred: {error}"),
    () => Console.WriteLine("Completed")
);
```

## 5. スレッドセーフなコレクション
`System.Collections.Concurrent` 名前空間には、ロック機構を備えたスレッドセーフなコレクションが用意されています。

|クラス名|説明|主なメソッド例|
|:-|:-|:-|
|`ConcurrentDictionary`| スレッドセーフなキーと値のペアのコレクション。|`TryAdd`、`TryGetValue`、`AddOrUpdate`|
|`ConcurrentQueue`| FIFO（First-In-First-Out）順序で要素を保持するスレッドセーフなキュー。|`Enqueue`、`TryDequeue`|
|`ConcurrentStack`| LIFO（Last-In-First-Out）順序で要素を保持するスレッドセーフなスタック。|`Push`、`TryPop` |
|`ConcurrentBag`| 順序に依存せずに要素を保持するスレッドセーフなコレクション。 | `Add`、`TryTake`|
|`BlockingCollection`|プロデューサー-コンシューマーパターンを実装するためのクラス。内部的に他のConcurrentコレクションを使用可能。 | `Add`、`Take``CompleteAdding` |

### Producer-Consumerパターン
Producer-Consumerパターンは、マルチスレッドプログラミングにおいて、プロデューサー（生産者）とコンシューマー（消費者）を連携させるデザインパターンです。このパターンは、複数のスレッドが安全かつ効率的にデータをやり取りするために使用されます。

#### 主な要素

1. **プロデューサー（Producer）**:
   - データを生成して共有リソース（通常はキュー）に追加する役割を担います。
   - 例: データの収集、イベントの生成、タスクの作成など。

2. **コンシューマー（Consumer）**:
   - 共有リソースからデータを取り出して処理する役割を担います。
   - 例: データの解析、タスクの実行、イベントの処理など。

3. **共有リソース（Shared Resource）**:
   - 通常はキューやバッファの形で実装され、プロデューサーとコンシューマーがデータをやり取りするための場所です。

#### 実装例（C#）

C#でのProducer-Consumerパターンの基本的な実装方法として`BlockingCollection<T>`を使用したパターンと`Channel`を使用したパターンがあります。

- `BlockingCollection<T>`を使用したパターン
```csharp
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading.Tasks;

public class TaskQueue
{
    private BlockingCollection<Action> _taskQueue = new BlockingCollection<Action>(boundedCapacity: 50);

    public TaskQueue()
    {
        Task.Run(() => ProcessTasks());
    }

    public void AddTask(Action task)
    {
        _taskQueue.Add(task);
    }

    private void ProcessTasks()
    {
        //タスクの実行はTask.Run()で作られたスレッドで行う
        foreach (var task in _taskQueue.GetConsumingEnumerable())
        {
            task();
        }
    }
}

class Program
{
    static void Main()
    {
        TaskQueue taskQueue = new TaskQueue();

        for (int i = 0; i < 10; i++)
        {
            int index = i;
            //タスクの追加はメインスレッドで行う
            taskQueue.AddTask(() => Console.WriteLine($"Processing task {index}"));
        }
    }
}
```

- `Channel`を使用したパターン
```csharp
using System;
using System.Threading.Channels;
using System.Threading.Tasks;

public class ProducerConsumer
{
    private readonly Channel<int> _channel = Channel.CreateBounded<int>(10);

    public async Task StartAsync()
    {
        var producerTask = ProduceAsync();
        var consumerTask = ConsumeAsync();

        await Task.WhenAll(producerTask, consumerTask);
    }

    private async Task ProduceAsync()
    {
        for (int i = 0; i < 20; i++)
        {
            await _channel.Writer.WriteAsync(i);
            Console.WriteLine($"Produced: {i}");
            await Task.Delay(100); // シミュレートするための遅延
        }
        _channel.Writer.Complete();
    }

    private async Task ConsumeAsync()
    {
        await foreach (var item in _channel.Reader.ReadAllAsync())
        {
            Console.WriteLine($"Consumed: {item}");
            await Task.Delay(150); // シミュレートするための遅延
        }
    }
}

class Program
{
    static async Task Main()
    {
        ProducerConsumer pc = new ProducerConsumer();
        await pc.StartAsync();
    }
}

```

## 6. Task

以下に、`Task`クラスの主要なプロパティとメソッドを表形式でまとめました。

### プロパティ

| プロパティ      | 説明                                                         | 例                                      |
|----------------|--------------------------------------------------------------|----------------------------------------|
| `Id`           | タスクの一意識別子                                            | `task.Id`                              |
| `Status`       | タスクの現在の状態（例: `WaitingToRun`, `Running`, `Completed`など） | `task.Status`                          |
| `IsCompleted`  | タスクが完了したかどうかを示すブール値                         | `task.IsCompleted`                     |
| `IsCanceled`   | タスクがキャンセルされたかどうかを示すブール値                 | `task.IsCanceled`                      |
| `IsFaulted`    | タスクがエラーによって失敗したかどうかを示すブール値           | `task.IsFaulted`                       |
| `Exception`    | タスクが失敗した場合にスローされた例外                         | `task.Exception`                       |
| `Result`       | タスクの完了後に返される結果（ジェネリック `Task<T>`の場合）    | `task.Result`                          |

### メソッド

| メソッド            | 説明                                                                 | 例                                                |
|--------------------|--------------------------------------------------------------------|--------------------------------------------------|
| `Run`              | 新しいタスクを作成して実行します                                      | `Task.Run(() => DoWork())`                       |
| `Wait`             | タスクが完了するまで現在のスレッドをブロックします                    | `task.Wait()`                                    |
| `WaitAll`          | 指定されたすべてのタスクが完了するまで待機します                      | `Task.WaitAll(task1, task2)`                     |
| `WaitAny`          | 指定された任意のタスクが完了するまで待機します                        | `Task.WaitAny(task1, task2)`                     |
| `ContinueWith`     | 現在のタスクが完了した後に実行する継続タスクを作成します              | `task.ContinueWith(t => Console.WriteLine("Task completed"))` |
| `FromResult`       | 指定された結果を持つ成功したタスクを作成します                        | `Task.FromResult(42)`                            |
| `FromException`    | 指定された例外を持つ失敗したタスクを作成します                        | `Task.FromException(new Exception("Error"))`     |
| `FromCanceled`     | 指定されたキャンセル状態を持つキャンセルされたタスクを作成します       | `Task.FromCanceled(new CancellationToken(true))` |
| `Delay`            | 指定された時間だけ遅延するタスクを作成します                          | `Task.Delay(1000)`                               |
| `WhenAll`          | 指定されたすべてのタスクが完了するまで待機するタスクを作成します       | `Task.WhenAll(task1, task2)`                     |
| `WhenAny`          | 指定された任意のタスクが完了するまで待機するタスクを作成します         | `Task.WhenAny(task1, task2)`                     |

`ConfigureAwait`は、C#の非同期プログラミングにおいて、`await`の動作をカスタマイズするためのメソッドです。具体的には、`await`の後に継続するコードが、呼び出し元のコンテキスト（通常はUIスレッド）で実行されるかどうかを制御します。以下に詳細を説明します。

### `ConfigureAwait()`について

#### `ConfigureAwait(true)`

- **デフォルトの動作**: `await`はデフォルトで呼び出し元のコンテキストを継承します。通常、UIスレッドなどのシンクロナイズコンテキストが保持されます。
- **例**:
  ```csharp
  public async Task ExampleAsync()
  {
      await Task.Delay(1000).ConfigureAwait(true);
      // 継続するコードは呼び出し元のコンテキスト（例えばUIスレッド）で実行されます
      UpdateUI();
  }
  ```

#### `ConfigureAwait(false)`

- **コンテキストの非継承**: `await`の後に続くコードが、呼び出し元のコンテキストを継承せずに実行されます。これにより、スレッド切り替えのオーバーヘッドが削減され、パフォーマンスが向上します。
- **例**:
  ```csharp
  public async Task ExampleAsync()
  {
      await Task.Delay(1000).ConfigureAwait(false);
      // 継続するコードは呼び出し元のコンテキストとは異なるスレッドで実行される可能性があります
      PerformBackgroundOperation();
  }
  ```

### ConfigureAwaitのメリット

1. **パフォーマンスの向上**:
   - `ConfigureAwait(false)`を使用することで、呼び出し元のコンテキストを維持するためのスレッド切り替えのオーバーヘッドを削減できます。特にバックグラウンド作業に適しています。

2. **デッドロックの回避**:
   - シンクロナイズコンテキストがUIスレッドである場合、`await`によるコンテキストの継承がデッドロックを引き起こすことがあります。`ConfigureAwait(false)`を使用することで、これを回避できます。

### 注意点

1. **UI更新が必要な場合**:
   - UIを更新するコードが`await`の後に続く場合、`ConfigureAwait(true)`を使用するか、適切なスレッドで実行するように注意が必要です。

2. **ライブラリコード**:
   - 一般的に、ライブラリコードでは`ConfigureAwait(false)`を使用することが推奨されます。これにより、ライブラリの利用者に依存しない非同期処理が可能になります。


