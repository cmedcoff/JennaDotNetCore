using System.Diagnostics;

namespace IocExample;

public class Container
{
    public string Name { get; private set; }
    public Container(string containerName)
    {
        Name = containerName;
        System.Diagnostics.Trace.TraceInformation("New instance of {0} created", Name);
    }
    
    private readonly System.Collections.Generic.Dictionary<Type, Type> _map = 
        new System.Collections.Generic.Dictionary<Type, Type>();

    public void Register<Interface,Implementation>()
    {
        try
        {
            _map.Add(typeof(Interface), typeof(Implementation));
            Trace.TraceInformation("Registering {0} for {1}", typeof(Interface).Name, typeof(Implementation).Name);
        }
        catch(Exception registerException)
        {
            Trace.TraceError("Mapping Exception", registerException);
            throw new Exception("Mapping Exception",registerException);
        }
    }

    public T Resolve<T>()
    {
        return (T)Resolve(typeof(T));
    }

    private object Resolve(Type type)
    {
        Type resolvedType;
        try
        {
            resolvedType = _map[type];
            Trace.TraceInformation("Resolving {0}", type.Name);
        }
        catch(Exception resolveException)
        {
            Trace.TraceError("Could't resolve type", resolveException);
            throw new Exception("Could't resolve type", resolveException);
        }

        var ctor = resolvedType.GetConstructors().First();
        var ctorParameters = ctor.GetParameters();
        if(ctorParameters.Length ==0)
        {
            Trace.TraceInformation("Constructor have no parameters");
            return Activator.CreateInstance(resolvedType);
        }

        var parameters = new System.Collections.Generic.List<object>();
        Trace.TraceInformation("Constructor found to have {0} parameters",ctorParameters.Length);

        foreach (var p in ctorParameters)
        {
            parameters.Add(Resolve(p.ParameterType));
        }

        return ctor.Invoke(parameters.ToArray());
    }
}