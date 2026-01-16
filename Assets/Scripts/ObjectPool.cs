using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectPool<T> where T : Component
{
    [SerializeField]
    private T prefab;
    [SerializeField]
    private int initialSize = 10;
    [SerializeField]
    private Transform parent;

    private readonly List<T> availableObjects = new List<T>();

    public void Initialize()
    {
        if (prefab == null)
        {
            throw new InvalidOperationException("Prefab is not set. Please set the prefab before initializing the pool.");
        }
        Preload();
    }
    public void Initialize(T prefab)
    {
        this.prefab = prefab;
        Preload();
    }
    public void Initialize(int initialSize)
    {
        this.initialSize = initialSize;
        Initialize();
    }
    public void Initialize(T prefab, Transform parent)
    {
        this.parent = parent;
        Initialize(prefab);
    }
    public void Initialize(T prefab, int initialSize)
    {
        this.initialSize = initialSize;
        Initialize(prefab);
    }
    public void Initialize(T prefab, Transform parent, int initialSize)
    {
        this.initialSize = initialSize;
        Initialize(prefab, parent);
    }

    public void Preload()
    {
        for (int i = 0; i < initialSize; i++)
        {
            T obj = UnityEngine.Object.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            availableObjects.Add(obj);
        }
    }

    public T Get(bool active = true)
    {
        if (availableObjects.Count > 0)
        {
            T obj = availableObjects[0];
            availableObjects.RemoveAt(0);
            obj.gameObject.SetActive(active);
            return obj;
        }
        else
        {
            T obj = UnityEngine.Object.Instantiate(prefab, parent);
            obj.gameObject.SetActive(active);
            return obj;
        }
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        availableObjects.Add(obj);
    }

    public void Clear()
    {
        foreach (var obj in availableObjects)
        {
            UnityEngine.Object.Destroy(obj.gameObject);
        }
        availableObjects.Clear();
    }
}
