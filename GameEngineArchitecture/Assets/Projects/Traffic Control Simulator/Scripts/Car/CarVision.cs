using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Component that uses forward-directed raycasting to
/// update a public CurrentObstacle property.
/// </summary>
public class CarVision : MonoBehaviour
{
    [SerializeField] private float visionDistance = 50f;
    [SerializeField] private Vector3 visionOffset = Vector3.zero;
    [SerializeField] private LayerMask obstacleLayerMask;
    public Obstacle CurrentObstacle { get; private set; }

    void Awake()
    {
        CurrentObstacle = new Obstacle(null, Vector3.zero);
    }

    void Update()
    {
        // Check for obstacles
        var t = transform;
        var ray = new Ray(t.position + visionOffset, t.forward);
        var hit = Physics.Raycast(ray, out var hitInfo, visionDistance, LayerMask.GetMask("Obstacle"));
        
        if (!hit)
        {
            CurrentObstacle = new Obstacle(null, Vector3.zero);
            //print("Reset obstacle");
            return;
        }
        
        var other = hitInfo.collider;
        if (other.gameObject == gameObject) return;
        
        // Just update hit position instead of wasting performance
        if (other.gameObject == CurrentObstacle.gameObject)
        {
            var obstacle = CurrentObstacle;
            obstacle.hitPosition = hitInfo.point;
            CurrentObstacle = obstacle;
            return;
        }
        
        /* We utilize tags for efficiency instead of using
         GetComponent and null checking, which is expensive */
        switch (other.tag)
        {
            case "Car":
                TrySetObstacle<Car>(other, hitInfo.point);
                break;
            case "TrafficLight":
                TrySetObstacle<TrafficLightPole>(other, hitInfo.point);
                break;
            default:
                CurrentObstacle = new Obstacle(other.gameObject, hitInfo.point);
                break;
        }
    }
    
    void TrySetObstacle<T>(Collider other, Vector3 hitPos = new()) where T : MonoBehaviour, IObstacle
    {
        if (other.TryGetComponent(out T component))
        {
            //print("Set obstacle: " + component.gameObject.name);
            CurrentObstacle = new Obstacle(component.gameObject, hitPos);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ExitPoint")) 
            GetComponent<Car>().Release();
    }

    // Draw vision debug lines
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        var t = transform;
        var p = t.position;
        Gizmos.DrawLine(p + visionOffset, p + visionOffset + t.forward * visionDistance);
        if (CurrentObstacle.hitPosition != Vector3.zero) Gizmos.DrawWireSphere(CurrentObstacle.hitPosition, 0.5f);
    }
}
