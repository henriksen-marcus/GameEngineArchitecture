using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinballGameMode : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private GameObject _ballSpawnPoint;
    public UI UI;
    private int score = 0;
    public float StartTime { get; private set; }
    public bool GameStarted { get; private set; }
    
    void Start()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    
    public void StartGame()
    {
        Instantiate(_ballPrefab, _ballSpawnPoint.transform.position, Quaternion.identity);
        StartTime = Time.time;
        GameStarted = true;
        print("StartGame");
    }

    public void AddScore(int amount)
    {
        score += amount;
        UI.SetScore(score);
    }
}
