namespace DependencyInjection.Services;

public class ConsoleMessageService : IMessageService
{
    public void SendMessage(string message) => Console.WriteLine(message);
}
