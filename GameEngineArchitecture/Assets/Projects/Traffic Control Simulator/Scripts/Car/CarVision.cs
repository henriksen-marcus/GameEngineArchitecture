using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CarVision : MonoBehaviour
{
    [SerializeField] private BoxCollider visionCollider;
    public Obstacle CurrentObstacle { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
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

    private void OnTriggerExit(Collider other)
    {
        // Check if it is our current obstacle
        var obstacle = CurrentObstacle;
        if (other.gameObject == obstacle.gameObject)
            obstacle.gameObject = null;
    }
    
    void TrySetObstacle<T>(Collider other) where T : MonoBehaviour, IObstacle 
    {
        // Check if the other collider is a car
        if (other.TryGetComponent(out T component))
        {
            CurrentObstacle = new Obstacle(component.gameObject);
        }
    }
}
