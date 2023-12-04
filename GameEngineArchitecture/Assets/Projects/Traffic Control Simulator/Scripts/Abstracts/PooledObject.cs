using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PooledObject : MonoBehaviour
{
    public ObjectPooler pool;
    public virtual void Release() => pool.ReturnToPool(this);
}
