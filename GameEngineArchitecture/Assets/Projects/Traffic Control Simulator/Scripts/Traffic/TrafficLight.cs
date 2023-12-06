using UnityEngine;

/// <summary>
/// The light itself inside a traffic light pole. It's
/// only responsibility is to change the light color/material.
/// </summary>
public class TrafficLight : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material onMaterial;
    [SerializeField] private Material offMaterial;
    
    private bool IsOn => meshRenderer.material == onMaterial;

    public void Switch()
    {
        meshRenderer.material = IsOn ? offMaterial : onMaterial;
    }
    
    void Start()
    {
        meshRenderer ??= GetComponent<MeshRenderer>();        
    }
}
