using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class TrafficRulesUI : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject speedLimitInput;
    [SerializeField] private GameObject accelerationInput;
    [SerializeField] private GameObject distanceInput;
    [SerializeField] private GameObject secondsInput;
    
    private void OnEnable()
    {
        speedLimitInput.GetComponent<TMP_InputField>().text = TrafficRuleManager.Instance.SpeedLimit.ToString(CultureInfo.InvariantCulture);
        accelerationInput.GetComponent<TMP_InputField>().text = TrafficRuleManager.Instance.MaxAcceleration.ToString(CultureInfo.InvariantCulture);
        distanceInput.GetComponent<TMP_InputField>().text = TrafficRuleManager.Instance.MinimumObstacleDistance.ToString(CultureInfo.InvariantCulture);
        secondsInput.GetComponent<TMP_InputField>().text = TrafficRuleManager.Instance.SecondsBehindObstacle.ToString(CultureInfo.InvariantCulture);
    }
    
    public void SaveTrafficRules()
    {
        float speedLimit = float.Parse(speedLimitInput.GetComponent<TMP_InputField>().text);
        float acceleration = float.Parse(accelerationInput.GetComponent<TMP_InputField>().text);
        float distance = float.Parse(distanceInput.GetComponent<TMP_InputField>().text);
        float seconds = float.Parse(secondsInput.GetComponent<TMP_InputField>().text);
        
        TrafficRuleManager.Instance.PushRulesChanges(new TrafficRules(speedLimit, acceleration, distance, seconds));
        parent.SetActive(false);
    }
}
