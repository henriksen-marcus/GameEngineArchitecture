using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingCube : PooledObject
{
    void Start()
    {
        
    }
    
    void Update()
    {
        transform.Translate(new Vector3(8,0,0)*Time.deltaTime, Space.World);
        if (transform.position.x > 40) Release();
        
    }
}
