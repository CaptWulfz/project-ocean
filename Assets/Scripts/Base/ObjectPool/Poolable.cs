using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Poolable : MonoBehaviour
{
    [SerializeField] protected ObjectPool pool;
    public void SetObjectPool(ObjectPool pool)
    {
        this.pool = pool;
    } 
    public ObjectPool GetObjectPool()
    {
        return this.pool;
    }

    public abstract void OnCreate();
    public abstract void OnGet();
    public abstract void OnReturn();
}
