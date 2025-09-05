using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    public GameObject _prefab;
    public List<GameObject> pool = new List<GameObject>();
    public Transform _parent;
    private Transform currentGo=null;

    public Pool(GameObject prefab, Transform parent)
    {
        _prefab = prefab;
        _parent = parent;
    }

    public void Init(GameObject prefab, Transform parent=null)
    {
        if (parent == null)
            _parent = null;
        else
            _parent = parent;
        _prefab = prefab;
    }

    public void Spawn(Vector3 position, bool activate=true)
    {
        currentGo = GiveMeAItem().transform;
        SpawnPos(position);
        currentGo.gameObject.SetActive(activate);
        currentGo = null;
    }

    public void Spawn(Quaternion rotation, bool activate = true)
    {
        currentGo = GiveMeAItem().transform;
        SpawnRot(rotation);
        currentGo.gameObject.SetActive(activate);
        currentGo = null;
    }

    public void Spawn(Vector3 position, Quaternion rotation, bool activate = true)
    {
        currentGo = GiveMeAItem().transform;
        SpawnPos(position);
        SpawnRot(rotation);
        currentGo.gameObject.SetActive(activate);
        currentGo = null;
    }

    public Transform SpawnAndReturn(Vector3 position, Quaternion rotation, bool activate = true)
    {
        currentGo = GiveMeAItem().transform;
        SpawnPos(position);
        SpawnRot(rotation);
        Transform result = currentGo;
        currentGo.gameObject.SetActive(activate);
        currentGo = null;
        return result;
    }


    public void KillItem(GameObject obj)
    {
        pool.Remove(obj);
    }

    [Tooltip("All setactive(false)")]
    public void ClearAll()
    {
        foreach (GameObject go in pool)
        {
            go.SetActive(false);
        }
    }

    private void SpawnPos(Vector3 position)
    {
        if (!currentGo)
            currentGo = GiveMeAItem().transform;
        currentGo.position = position;
    }

    private void SpawnRot(Quaternion rotation)
    {
        if (!currentGo)
            currentGo = GiveMeAItem().transform;
        currentGo.rotation = rotation;

    }

    public GameObject GiveMeAItem()
    {
        foreach (GameObject go in pool)
        {
            if (!go.activeInHierarchy)
                return go;
        }
        GameObject newGo = GameObject.Instantiate(_prefab, _parent);
        pool.Add(newGo);
        return newGo;
    }


}
