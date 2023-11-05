using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RainSimManager : MonoBehaviour
{
    [SerializeField] GameObject rainDropToSpawn;
    [SerializeField] private Bounds spawnBounds = new Bounds(new Vector3(), new Vector3(10, 0, 10));
    [SerializeField] private float timeToDespawn = 4f;
    /// <summary>
    /// How much time between rain drop spawns.
    /// </summary>
    [SerializeField] private float SpawnRate = 0.1f;
    /// <summary>
    /// How many rain drops to spawn each time spawnRate has passed.
    /// </summary>
    [SerializeField] private int spawnAmount = 1;
    
    void Start()
    {
        InvokeRepeating(nameof(SpawnRainDrop), 0, 0.1f);
    }


    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, spawnBounds.size);
    }

    Vector3 GetRandPosition()
    {
        var pos = new Vector3();
        
        pos.x = Random.Range(spawnBounds.min.x, spawnBounds.max.x);
        pos.y = gameObject.transform.position.y;
        pos.z = Random.Range(spawnBounds.min.z, spawnBounds.max.z);
        
        return pos;
    }


    void SpawnRainDrop()
    {
        for (var i = 0; i < spawnAmount; i++)
        {
            var raindrop = Instantiate(rainDropToSpawn, GetRandPosition(), Quaternion.identity);
            Destroy(raindrop, timeToDespawn);
        }
    }
}
