using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarSpawningManager : Singleton<CarSpawningManager>
{
    private CarPortal[] carSpawners;
    //public event Action<T> ruleUpdateEvent;
    
    void Start()
    {
        // Start invoking a timer for each spawner
        
        // Get a reference to all the spawners
        carSpawners = FindObjectsOfType<CarPortal>();
    }
    
    void Update()
    {
        
    }
}
