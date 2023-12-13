using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficRuleManager : Singleton<TrafficRuleManager>
{
    [SerializeField] private GameObject trafficRulesUI;
    [SerializeField] private float speedLimit = 10;
    [SerializeField] private float maxAcceleration = 1;
    [SerializeField] private float minimumObstacleDistance = 3;
    [SerializeField] private float secondsBehindObstacle = 3;
    private TrafficRules _trafficRules;
    
    public event Action<TrafficRules> RuleUpdateEvent;
    
    public float SpeedLimit => _trafficRules.SpeedLimit;
    public float MaxAcceleration => _trafficRules.MaxAcceleration;
    public float MinimumObstacleDistance => _trafficRules.MinimumObstacleDistance;
    public float SecondsBehindObstacle => _trafficRules.SecondsBehindObstacle;
    
    void Start()
    {
        _trafficRules = new TrafficRules(speedLimit, maxAcceleration, minimumObstacleDistance, secondsBehindObstacle);
        PushRulesChanges(_trafficRules);
    }

    public void PushRulesChanges(TrafficRules rules)
    {
        _trafficRules = rules;
        OnRuleUpdateEvent(rules);
    }

    protected virtual void OnRuleUpdateEvent(TrafficRules rules)
    {
        RuleUpdateEvent?.Invoke(rules);
    }
    
    public void ShowRulesUI()
    {
        trafficRulesUI.SetActive(true);
    }
}
