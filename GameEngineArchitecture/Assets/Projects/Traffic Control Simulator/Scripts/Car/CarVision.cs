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
    private Vector3 lastpos;

    void Awake()
    {
        CurrentObstacle = new Obstacle(null);
    }

    void Update()
    {
        var t = transform;
        var ray = new Ray(t.position + visionOffset, t.forward);
        var hit = Physics.Raycast(ray, out var hitInfo, visionDistance, LayerMask.GetMask("Obstacle"));
        
        if (!hit)
        {
            CurrentObstacle = new Obstacle(null);
            return;
        }
        
        lastpos = hitInfo.point;
        var other = hitInfo.collider;
        if (other.gameObject == gameObject || other.gameObject == CurrentObstacle.gameObject) return;
        
        /* We utilize tags for efficiency instead of using
         GetComponent and null checking, which is expensive */
        switch (other.tag)
        {
            case "Car":
                TrySetObstacle<Car>(other);
                break;
            case "TrafficLight":
                TrySetObstacle<TrafficLightPole>(other);
                break;
        }
    }
    
    void TrySetObstacle<T>(Collider other) where T : MonoBehaviour, IObstacle
    {
        
        // Check if the other collider is a car
        if (other.TryGetComponent(out T component))
        {
            print("Set obstacle: " + component.gameObject.name);
            CurrentObstacle = new Obstacle(component.gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        var t = transform;
        var p = t.position;
        Gizmos.DrawLine(p + visionOffset, p + visionOffset + t.forward * visionDistance);
        Gizmos.DrawWireSphere(lastpos, 0.5f);
    }
}
