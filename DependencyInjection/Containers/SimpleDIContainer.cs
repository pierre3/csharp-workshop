namespace DependencyInjection.Containers;

public class SimpleDIContainer
{
    private readonly Dictionary<Type, Func<object[], object>> _registrations = new();

    public void Register<TService>(Func<object[], TService> factory) where TService : notnull
    {
        _registrations[typeof(TService)] = args => factory(args);
    }

    public TService Resolve<TService>(params object[] args)
    {
        if (_registrations.TryGetValue(typeof(TService), out var factory))
        {
            return (TService)factory(args);
        }
        throw new InvalidOperationException($"Service type {typeof(TService)} is not registered.");
    }

}
