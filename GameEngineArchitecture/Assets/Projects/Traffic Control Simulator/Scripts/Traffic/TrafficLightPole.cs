using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TrafficLightPole : MonoBehaviour, IClickable
{
    [SerializeField] private float greenTime = 5f;
    [SerializeField] private float redTime = 5f;
    [SerializeField] private GameObject redLight;
    [SerializeField] private GameObject greenLight;
    [SerializeField] private TrafficLightState currentState = TrafficLightState.Red;
    
    private TrafficLight _redLight;
    private TrafficLight _greenLight;

    private bool IsGreen
    {
        get => currentState == TrafficLightState.Green;
        set => currentState = value ? TrafficLightState.Green : TrafficLightState.Red;
    }
    
    private void Awake()
    {
        TrafficLightManager.Instance.RuleUpdateEvent += HandleRuleChange;
    }

    public void OnClicked()
    {
        // Spawn UI
        Debug.Log("Traffic light clicked!");
    }
    
    void Start()
    {
        _redLight = redLight.GetComponent<TrafficLight>();
        _greenLight = greenLight.GetComponent<TrafficLight>();
    }
    
    IEnumerator TrafficLightTimer()
    {
        while (true)
        {
            // Wait for the current state duration
            yield return new WaitForSeconds(IsGreen ? greenTime : redTime);

            // Switch state
            IsGreen = !IsGreen;

            // Change the color based on the current state
            _redLight.Switch();
            _greenLight.Switch();
        }
    }

    void HandleRuleChange(float _greenTime, float _redTime)
    {
        this.greenTime = _greenTime;
        this.redTime = _redTime;
    }
}
