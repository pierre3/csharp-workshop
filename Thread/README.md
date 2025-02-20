# マルチスレッドプログラミング

## スレッドとは
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

### Threadクラス
Threadを扱うための基本クラス

```csharp
using System.Threading;

Thread myThread = new Thread(MyThreadMethod);
myThread.Start();

void MyThreadMethod()
{
    // スレッドで実行する処理
}
```

## スレッドの管理
### ThreadPool
生成コストの高いThreadを効率的に使うための仕組み。予め生成しておいた複数のThreadを使いまわす。


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

### スレッドの同期
同じメモリ空間を共有しているため、スレッド間のデータ競合を防ぐ仕組みが必要。

- lock文
- Monitor

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
- デッドロック:  
  ロックを取得した後に他のリソースのロックを待つとデッドロックが発生する可能性があるため、注意が必要

- パフォーマンス:   
  過剰なロックはパフォーマンスの低下を招く可能性がある


## C#でのマルチスレッドプログラミング

### なまThread＆ThreadPool
スレッドの基本クラス。こちらを直接利用することはほとんどないと思います。

```csharp
using System.Threading;

Thread myThread = new Thread(MyThreadMethod);
myThread.Start();

void MyThreadMethod()
{
    // スレッドで実行する処理
}
```

```csharp
ThreadPool.QueueUserWorkItem(new WaitCallback(MyThreadPoolMethod));

void MyThreadPoolMethod(object state)
{
    Console.WriteLine("スレッドプールで実行されています。");
}

```

### Delegateを利用した非同期処理
デリゲートを使用して非同期処理を実装することができます。BeginInvokeメソッドを使用して非同期にメソッドを呼び出し、EndInvokeメソッドで結果を取得します。

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
タスクベースの非同期パターン (TAP) 。`Task` クラスと `async`/`await` キーワードは、非同期プログラミングを簡単に実装するための強力なツールです。

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
