using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject original;

    [HideInInspector] public List<GameObject> objectPool = new List<GameObject>();

    public GameObject SpawnObject()
    {
        for (int i = 0; i < objectPool.Count; ++i)
        {
            if (objectPool[i].activeSelf) continue;

            objectPool[i].SetActive(true);

            return objectPool[i];
        }

        GameObject newObject = Instantiate(original, transform);

        objectPool.Add(newObject);

        return newObject;
    }

    public T SpawnObject<T>() where T : Component
    {
        GameObject newGO = SpawnObject();

        return newGO.GetComponent<T>();
    }

    public GameObject GetObject(int index)
    {
        if (index < 0) return null;

        if (index >= objectPool.Count) return null;

        return objectPool[index];
    }

    public void DestroyObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void DestroyAll()
    {
        for (int i = 0; i < objectPool.Count; ++i) 
        {
            if (objectPool[i].activeSelf) objectPool[i].SetActive(false);
        }
    }

    public void LoopThroughActiveObjects<T>(Action<T> action) where T : Component
    {
        for (int i = 0; i < objectPool.Count; ++i)
        {
            if (objectPool[i].activeSelf == false) continue;

            var component = objectPool[i].GetComponent<T>();

            action.Invoke(component);
        }
    }
}
