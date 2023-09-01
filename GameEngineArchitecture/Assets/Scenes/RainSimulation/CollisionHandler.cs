using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.LogWarning($"Collided with {other.gameObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning($"Overlapped with {other.gameObject.name}");
    }
}
