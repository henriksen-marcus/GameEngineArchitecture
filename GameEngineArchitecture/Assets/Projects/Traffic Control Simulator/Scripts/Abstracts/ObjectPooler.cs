using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

/// <summary>
/// Component that manages a pool of objects.
/// </summary>
public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private uint poolSize;
    [SerializeField] private PooledObject objectToPool;
    [SerializeField] private bool setupOnStart = true;
    protected readonly Stack<PooledObject> Stack = new Stack<PooledObject>();
    //ObjectPool<PooledObject> pool;
    
    protected virtual void Start()
    {
        if (setupOnStart) SetupPool();
    }

    /// <summary>
    /// Fills the pool with the poolSize amount of deactivated objects.
    /// </summary>
    public virtual void SetupPool()
    {
        for (var i = 0; i < poolSize; i++)
        {
            var obj = Instantiate(objectToPool);
            obj.pool = this;
            obj.gameObject.SetActive(false);
            Stack.Push(obj);
        }
    }
    
    /// <summary>
    /// Retrieves an object from the pool, removing it from the stack
    /// and activating it.
    /// </summary>
    public virtual PooledObject GetObject()
    {
        if (Stack.Count == 0) return SpawnObject();
        
        var obj = Stack.Pop();
        obj.gameObject.SetActive(true);
        return obj;
    }
    
    protected virtual PooledObject SpawnObject()
    {
        PooledObject obj = Instantiate(objectToPool);
        obj.pool = this;
        return obj;
    }
    
    /// <summary>
    /// Pushes the object back in the stack and deactivates it.
    /// </summary>
    public void ReturnToPool(PooledObject obj)
    {
        Stack.Push(obj);
        obj.gameObject.SetActive(false);
    }
}