namespace DependencyInjection.Services;

public class GreetingService(IMessageService messageService)
{
    private readonly string _additionalMessage = "";

    public GreetingService(IMessageService messageService, string additionalMessage) : this(messageService)
        => _additionalMessage = additionalMessage;

    public void Greet(string name) => messageService.SendMessage($"Hello, {name}! {_additionalMessage}");
}
