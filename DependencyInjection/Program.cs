using DependencyInjection.Services;
using DependencyInjection.Containers;

namespace DependencyInjection;

// 実行例
class Program
{
    static void Main()
    {
        var container = new SimpleDIContainer();
        container
            .AddMessageService<ConsoleMessageService>()
            .AddGreetingService();

        var greetingService = container.Resolve<GreetingService>(
            container.Resolve<IMessageService>(),
            "How are things going?");
        greetingService?.Greet("Hayato");
    }
}
