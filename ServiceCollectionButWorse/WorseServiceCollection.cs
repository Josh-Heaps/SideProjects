using System.Reflection;

namespace ServiceCollectionButWorse;

public class WorseServiceCollection
{
    private readonly Dictionary<Type, ConstructorInfo> _types = [];
    private readonly Dictionary<Type, object> _cache = [];

    public WorseServiceCollection() { }

    public WorseServiceCollection(params Type[] types)
    {
        AddServices(types);
    }

    public void AddService<T>()
    {
        AddService(typeof(T));
    }

    public void AddService(Type type)
    {
        if (type.GetConstructors().Length > 1)
            throw new ArgumentException($"Type {type.Name} has multiple constructors. Only one constructor per type allowed.");

        _types.Add(type, type.GetConstructors()[0]);
    }

    public void AddServices(params Type[] types)
    {
        foreach (var type in types)
        {
            AddService(type);
        }
    }

    public T GetService<T>()
    {
        return (T)GetService(typeof(T));
    }

    public object GetService(Type type)
    {
        return GetService(type, nonAllowedDependencies: []);
    }

    private object GetService(Type type, params Type[] nonAllowedDependencies)
    {
        if (_cache.TryGetValue(type, out var obj))
            return obj;

        if (!_types.TryGetValue(type, out var constructor))
            throw new ArgumentException($"Type {type.Name} not registered");

        return BuildService(type, constructor, nonAllowedDependencies);
    }

    private object BuildService(Type type, ConstructorInfo constructor, params Type[] nonAllowedDependencies)
    {
        var paramInfo = constructor.GetParameters();

        object[] parameters = [];

        if (paramInfo.Length > 0)
            parameters = BuildDependencies(paramInfo, [.. nonAllowedDependencies, type]);

        var instance = constructor.Invoke(parameters);
        _cache.TryAdd(type, instance);

        return instance;
    }

    private object[] BuildDependencies(ParameterInfo[] paramInfo, params Type[] nonAllowedDependencies)
    {
        var requiredTypes = paramInfo.Select(p => p.ParameterType).ToArray();

        if (requiredTypes.Any(x => nonAllowedDependencies.Contains(x)))
            throw new ArgumentException($"Circular dependency detected involving" +
                $" {requiredTypes.First(x => nonAllowedDependencies.Contains(x)).Name}");

        List<object> instances = [];

        foreach (var requiredType in requiredTypes)
        {
            instances.Add(GetService(requiredType, nonAllowedDependencies));
        }

        object[] orderedDependencies = new object[paramInfo.Length];

        foreach (var param in paramInfo)
        {
            orderedDependencies[param.Position] = instances.First(x => x.GetType() == param.ParameterType);
        }

        return orderedDependencies;
    }
}
