using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform raycastPoint;
    
    public Transform SpawnPoint => spawnPoint;
    
    public bool CanSpawn()
    {
        // Check if there is a car in the way
        var ray = new Ray(raycastPoint.position, raycastPoint.forward);
        if (Physics.Raycast(ray, out var hit, 10f))
        {
            return false;
        }

        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(raycastPoint.position, raycastPoint.forward * 10f);
    }
}
