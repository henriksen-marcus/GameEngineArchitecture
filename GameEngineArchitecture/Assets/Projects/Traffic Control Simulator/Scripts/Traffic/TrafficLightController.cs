using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller for a group of traffic lights in a crossing.
/// Opposite directions switch simultaneously.
/// </summary>
public class TrafficLightController : MonoBehaviour, IClickable
{
    [SerializeField] private TrafficLightPole[] trafficLights;
    [SerializeField] private GameObject trafficControllerUI;

    public float GreenTime  = 5;
    public float RedTime = 5;
    
    public event Action<float, float> RuleUpdateEvent;

    private void Start()
    {
        PushRuleChanges();
    }

    protected virtual void OnRuleUpdateEvent(float greenTime, float redTime)
    {
        RuleUpdateEvent?.Invoke(greenTime, redTime);
    }
    
    public void OnClicked()
    {
        // This should be reworked later
        trafficControllerUI.GetComponentInChildren<TrafficControllerUI>().trafficLightController = this;
        trafficControllerUI.SetActive(true);
    }

    public void PushRuleChanges()
    {
        OnRuleUpdateEvent(Mathf.Clamp(GreenTime, 1, 50), Mathf.Clamp(RedTime, 1, 50));
    }
}
