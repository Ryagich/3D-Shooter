using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    public bool AutoExpand = true;

    private readonly Transform parent;
    private readonly HashSet<GameObject> inUse;
    private readonly HashSet<GameObject> unused;
    private readonly GameObject prefab;

    public Pool(GameObject prefab, int size, Transform parent = null)
    {
        this.prefab = prefab;
        inUse = new HashSet<GameObject>();
        unused = new HashSet<GameObject>();
        for (int i = 0; i < size; i++)
        {
            var obj = GameObject.Instantiate(prefab, parent);
            obj.SetActive(false);
            unused.Push(obj);
        }
    }

    public IEnumerable<GameObject> GetActive() => inUse;

    public bool IsAvailable() => unused.Count > 0;

    public GameObject Get()
    {
        GameObject obj;
        if (unused.Count <= 0 && AutoExpand)
        {
            obj = GameObject.Instantiate(prefab, parent);
            inUse.Push(obj);
        }
        else
        {
            obj = unused.Pop();            
        }
        obj.SetActive(true);
        inUse.Push(obj);
        return obj;
    }

    public void Return(GameObject obj)
    {
        if (inUse.Contains(obj))
        {
            inUse.Remove(obj);
            unused.Add(obj);
            obj.transform.position = prefab.transform.position;
            obj.transform.localScale = prefab.transform.localScale;
            obj.transform.rotation = prefab.transform.rotation;
            obj.SetActive(false);
        }
    }
}

public static class HashSetExtention
{
    public static T Pop<T>(this HashSet<T> t)
    {
        if (t.Count <= 0)
            throw new Exception(t.Count.ToString());
        var obj = t.First();
        t.Remove(obj);
        return obj;
    }

    public static void Push<T>(this HashSet<T> t, T obj)
    {
        if (!t.Contains(obj))
            t.Add(obj);
    }
}
