using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : PooledObject, IProduct
{
    public string Name => "Moving Cube";
    
    public void Initialize()
    {
        print("Moving Cube Initialized");
    }
    
    void Update()
    {
        transform.Translate(new Vector3(8,0,0)*Time.deltaTime, Space.World);
        if (transform.position.x > 40) Release();
        
    }
}
