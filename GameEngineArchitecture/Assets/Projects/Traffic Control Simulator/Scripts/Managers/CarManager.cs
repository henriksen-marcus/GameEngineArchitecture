using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class CarManager : Singleton<CarManager>
{
    protected ObjectPooler ObjectPooler;
    
    void Start()
    {
        ObjectPooler = GetComponent<ObjectPooler>();
        ObjectPooler.SetupPool();
    }
    
    void Update()
    {
        
    }
}
