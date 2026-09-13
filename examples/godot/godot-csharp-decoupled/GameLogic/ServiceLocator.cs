namespace GameLogic;

/// <summary>
/// Simple service locator for dependency injection.
/// Services are registered at startup (in Main.cs) and consumed by gameplay scripts.
/// This avoids tight coupling between gameplay nodes and Godot-specific implementations.
/// </summary>
public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new();

    public static void Register<T>(T service) where T : class
    {
        _services[typeof(T)] = service;
    }

    public static T Get<T>() where T : class
    {
        if (_services.TryGetValue(typeof(T), out var service))
            return (T)service;

        throw new InvalidOperationException($"Service {typeof(T).Name} not registered.");
    }

    public static void Clear()
    {
        _services.Clear();
    }
}