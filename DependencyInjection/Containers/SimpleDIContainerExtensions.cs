using DependencyInjection.Services;

namespace DependencyInjection.Containers;

static class SimpleDIContainerExtensions
{
    public static SimpleDIContainer AddMessageService<TMessageService>(this SimpleDIContainer container) 
        where TMessageService : IMessageService, new()
    {
        container.Register<IMessageService>(_ => new TMessageService());
        return container;
    }

    public static SimpleDIContainer AddGreetingService(this SimpleDIContainer container)
    {
        container.Register(args =>
        {
            switch (args.Length)
            {
                case 1 when args[0] is IMessageService messageService:
                    return new GreetingService(messageService);
                case 2 when args[0] is IMessageService messageService && args[1] is string additionalMessage:
                    return new GreetingService(messageService, additionalMessage);
                default:
                    throw new InvalidOperationException("引数が一致するコンストラクタがありません。");
            }
        });
        return container;
    }
}