using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficRulesEnabler : MonoBehaviour
{
    [SerializeField] private GameObject trafficRulesUI;
    
    public void EnableTrafficRules()
    {
        trafficRulesUI.SetActive(true);
    }
}
