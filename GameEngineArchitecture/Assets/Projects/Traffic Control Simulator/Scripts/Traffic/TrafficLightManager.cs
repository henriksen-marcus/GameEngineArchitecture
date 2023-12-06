using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightManager : Singleton<TrafficLightManager>
{
    public event Action<float, float> RuleUpdateEvent;
    public float DefaultGreenTime = 10f;
    public float DefaultRedTime = 3f;
    
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    protected virtual void OnRuleUpdateEvent(float greenTime, float redTime)
    {
        RuleUpdateEvent?.Invoke(greenTime, redTime);
    }
}
