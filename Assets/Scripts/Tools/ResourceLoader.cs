using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader
{
    Dictionary<string, object> cachedResources = new Dictionary<string, object>();

    private T LoadResource<T>(ref string resource, bool cache)
    {
        object objectLoaded = Resources.Load(resource, typeof(T));
        if (cache)
            cachedResources.Add(resource, objectLoaded);
        return (T)objectLoaded;
    }

    public T GiveMeAResource<T>(string resource, bool cache = false)
    {
        if (cachedResources.ContainsKey(resource))
            return (T)cachedResources[resource];
        return LoadResource<T>(ref resource, cache); ;
    }

    public GameObject Instantiate(string resource, Transform parent, bool cache = true)
    {
        GameObject go = null;
        try
        {
            go = GiveMeAResource<GameObject>(resource, cache);
        }
        catch
        {
            Debug.Log(resource + " is not a GameObject or does not exists");
        }
        return GameObject.Instantiate(go, parent);
    }

    public void ClearCache()
    {
        cachedResources.Clear();
    }
}