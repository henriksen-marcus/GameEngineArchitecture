using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class CarManager : Singleton<CarManager>
{
    [SerializeField] private float spawnRate = 3f;
    [SerializeField] private bool spawnAllOnStart = true;
    [SerializeField] private CarSpawner[] spawners;
    protected ObjectPooler ObjectPooler;
    private int _currentIndex = 0;
    
    public bool ShouldKeepSpawning { get; set; } = true;
    
    void Start()
    {
        ObjectPooler = GetComponent<ObjectPooler>();
        ObjectPooler.SetupPool();
        
        if (spawnAllOnStart)
        {
            for (int i = 0; i < spawners.Length; i++)
                SpawnCar();
        }
        
        StartCoroutine(WaitSpawnCar());
    }
    
    
    IEnumerator WaitSpawnCar()
    {
        while (ShouldKeepSpawning)
        {
            yield return new WaitForSeconds(spawnRate);
            SpawnCar();
        }
    }

    void SpawnCar()
    {
        // Check if we can spawn a car
        if (ObjectPooler.RemainingObjects <= 0 || !spawners[_currentIndex].CanSpawn()) return;
        
        var car = ObjectPooler.GetObject();
        var t = spawners[_currentIndex].SpawnPoint;
        car.transform.position = t.position;
        car.transform.rotation = t.rotation;
        
        // Change spawning point
        _currentIndex = _currentIndex == spawners.Length - 1 ? 0 : _currentIndex + 1;
    }
}
