using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// The light itself inside a traffic light pole. It's
/// only responsibility is to change the light color/material.
/// </summary>
public class TrafficLight : MonoBehaviour
{
    [SerializeField] private Material onMaterial;
    [SerializeField] private Material offMaterial;
    private Renderer _renderer;
    public bool isOn = false;

    public void Switch()
    {
        _renderer.material = isOn ? offMaterial : onMaterial;
        isOn = !isOn;
    }
    
    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer) _renderer.material = offMaterial;
        else Debug.LogError($"{gameObject.name}: Missing mesh renderer!");
    }
}
