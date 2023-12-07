using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TrafficLightPole : MonoBehaviour, IClickable, IObstacle
{
    [SerializeField] private float greenTime = 5f;
    [SerializeField] private float redTime = 5f;
    [SerializeField] private GameObject redLight;
    [SerializeField] private GameObject greenLight;
    [SerializeField] private TrafficLightState currentState = TrafficLightState.Red;
    
    private TrafficLight _redLight;
    private TrafficLight _greenLight;
    private float overrideGreenTime = 0f;
    private float overrideRedTime = 0f;

    public Rigidbody Rigidbody => null;

    public bool IsGreen
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
        
        // Disable object if the light references are missing
        if (_redLight == null || _greenLight == null)
        {
            Debug.LogError("TrafficLightPole: Missing traffic light component!");
            gameObject.SetActive(false);
            return;
        }
        
        switch (currentState)
        {
            case TrafficLightState.Green:
                _greenLight.Switch();
                break;
            case TrafficLightState.Red:
                _redLight.Switch();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        StartCoroutine(TrafficLightTimer());
    }
    
    IEnumerator TrafficLightTimer()
    {
        while (true)
        {
            // If the user has overriden the time in UI, use that instead
            float newRedTime = overrideRedTime > 0 ? overrideRedTime : redTime;
            float newGreenTime = overrideGreenTime > 0 ? overrideGreenTime : greenTime;
            
            // Wait for the current state duration
            //print($"Wait for: {(IsGreen ? greenTime : redTime)}");
            yield return new WaitForSeconds(IsGreen ? newGreenTime : newRedTime);

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
