using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator 
{
    public static ServiceLocator Instance => _instance ?? (_instance = new ServiceLocator());
    private static ServiceLocator _instance;

    private readonly Dictionary<Type, object> _services;

    public ServiceLocator()
    {
        _services = new Dictionary<Type, object>();
    }

    public void RegisterService<T> (T service)
    {
        var type = typeof(T);
        if (!_services.ContainsKey(type))
        {
            _services.Add(type, service);
        }
        else
            Debug.Log("Intentando registrar una clase ya registrada");
    }

    public T GetService<T>()
    {
        var type = typeof(T);
        if (!_services.TryGetValue(type, out var service))
            throw new Exception("Servicio no encontrado");

        return (T)service;
    }

    public void Clear()
    {
        _services.Clear();
    }

}
