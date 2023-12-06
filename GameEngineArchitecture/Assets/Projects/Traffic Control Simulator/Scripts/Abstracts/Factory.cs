using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for factories.
/// </summary>
/// <typeparam name="T">The type of product the factory should make.</typeparam>
public abstract class Factory<T> : MonoBehaviour where T : Object, IProduct
{
    [SerializeField] protected T productPrefab;

    public virtual IProduct GetProduct()
    {
        var product = Instantiate(productPrefab);
        product.Initialize();
        return product;
    }
        
}
