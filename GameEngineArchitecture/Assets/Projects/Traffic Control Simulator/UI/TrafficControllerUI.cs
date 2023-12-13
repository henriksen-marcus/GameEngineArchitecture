using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TrafficControllerUI : MonoBehaviour
{
    public TrafficLightController trafficLightController;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject greenTimeInput;
    [SerializeField] private GameObject redTimeInput;

    private void OnEnable()
    {
        greenTimeInput.GetComponent<TMP_InputField>().text = trafficLightController.GreenTime.ToString(CultureInfo.InvariantCulture);
        redTimeInput.GetComponent<TMP_InputField>().text = trafficLightController.RedTime.ToString(CultureInfo.InvariantCulture);
        //("GreenTime: " +trafficLightController.GreenTime);
    }

    public void Save()
    {
        float greenTime = float.Parse(greenTimeInput.GetComponent<TMP_InputField>().text);
        float redTime = float.Parse(redTimeInput.GetComponent<TMP_InputField>().text);
        
        trafficLightController.GreenTime = greenTime;
        trafficLightController.RedTime = redTime;
        trafficLightController.PushRuleChanges();
        parent.SetActive(false);
    }
}
