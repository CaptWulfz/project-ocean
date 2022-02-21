using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public bool generateOnAwake;
    public GameObject template;
    public int poolSize;
    public Transform poolTransform;
    public List<GameObject> activePool, inactivePool;
    
    public void Awake()
    {
        if(generateOnAwake)
            GeneratePool();
    }

    //Generates Object pool. Adds <poolSize> amount of <Template>.
    public void GeneratePool()
    {
        activePool = new List<GameObject>();
        inactivePool = new List<GameObject>();
        
        for(int i = 0; i < poolSize; i++)
        {
            GameObject poolObject = Instantiate(template, poolTransform);
            poolObject.SetActive(false);

            inactivePool.Add(poolObject);

            //OnCreate function is called if Poolable component exists.
            Poolable poolable = poolObject.GetComponent<Poolable>();
            if (poolable == null)
                continue;

            poolable.SetObjectPool(this);
            poolable.OnCreate();
        }
    }

    //Gets and activates one item from the Object pool.
    public GameObject GetPoolObject()
    {
        GameObject poolObject = inactivePool[0];
        poolObject.SetActive(true);

        //OnGet function is called if Poolable component exists.
        Poolable poolable = poolObject.GetComponent<Poolable>();
        if (poolable != null)
            poolable.OnGet();

        activePool.Add(poolObject);
        inactivePool.Remove(poolObject);

        return poolObject;
    }

    public List<GameObject> EditAllPoolObjects()
    {
        List<GameObject> poolObjects = new List<GameObject>();
        poolObjects.AddRange(inactivePool);
        poolObjects.AddRange(activePool);
        return poolObjects;
    }

    //Returns and deactivates an item in the Object pool.
    public void ReturnPoolObject(GameObject poolObject)
    {
        if (!activePool.Contains(poolObject))
            return;

        //OnReturn function is called if Poolable component exists.
        Poolable poolable = poolObject.GetComponent<Poolable>();
        if (poolable != null)
        {
            poolable.OnReturn();
            poolable.SetObjectPool(this);
        }

        poolObject.SetActive(false);

        inactivePool.Add(poolObject);
        activePool.Remove(poolObject);
    }

    public void ReturnPoolObject(List<GameObject> poolObjects)
    {
        foreach (GameObject poolObject in poolObjects)
            ReturnPoolObject(poolObject);
    }
}
