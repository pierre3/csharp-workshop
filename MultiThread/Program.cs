/*
 Sample of System.Threading.Channels
 */

using System.Threading.Channels;

Channel<string> channel = Channel.CreateUnbounded<string>();
Console.WriteLine("Please input any words. To exit, input 'x'.");

var defColor = Console.ForegroundColor;

var producer1 = Task.Run(async () =>
{
    while (true)
    {
        var input = Console.ReadLine() ?? "";
        if (input == "x")
        {
            channel.Writer.Complete();
            break;
        }
        await channel.Writer.WriteAsync(input);
    }
});

//var producer2 = Task.Run(async () =>
//{
//    while (true)
//    {
//        await Task.Delay(2000);
//        var time = DateTime.Now.ToString("HH:mm:ss");

//        if (!channel.Writer.TryWrite(time))
//        {
//            break;
//        }
//    }
//});


var consumer1 = Task.Run(async () =>
{
    while (await channel.Reader.WaitToReadAsync())
    {
        if (channel.Reader.TryRead(out var message))
        {
            await Task.Delay(3000);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[1] > " + message);
            Console.ForegroundColor = defColor;
        }
    }
});

//var consumer2 = Task.Run(async () =>
//{
//    while (await channel.Reader.WaitToReadAsync())
//    {
//        if (channel.Reader.TryRead(out var message))
//        {
//            await Task.Delay(6000);
//            Console.ForegroundColor = ConsoleColor.Green;
//            Console.WriteLine("[2] > " + message);
//            Console.ForegroundColor = defColor;
//        }
//    }
//});
await Task.WhenAll(producer1, consumer1);
//await Task.WhenAll(producer1, consumer1, consumer2);
//await Task.WhenAll(producer1, producer2, consumer1, consumer2);
Console.WriteLine(">> exit.");