using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DependencyInjector
{
    private static DependencyInjector _instance = null;

    private Dictionary<object, ISystemComponent> _systemComponents = new Dictionary<object, ISystemComponent>();

    public static DependencyInjector Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DependencyInjector();
            }

            return _instance;
        }
    }

    public DependencyInjector()
    {
        _systemComponents.Add(typeof(MessageExchange), new MessageExchange());
    }

    public T GetSystem<T>() where T : ISystemComponent
    {
        ISystemComponent foundComponent;

        if (!_systemComponents.TryGetValue(typeof(T), out foundComponent))
        {
            Debug.LogError(string.Format("[Dependency Injector] Could not find reference to {0}", typeof(T)));
        }

        return (T)foundComponent;
    }
}
