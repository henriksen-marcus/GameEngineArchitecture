using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    [SerializeField] PinballGameMode _gameMode;
    Label _scoreLabel;
    Label _timeLabel;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        
        _scoreLabel = root.Q<Label>("ScoreNumber");
        _timeLabel = root.Q<Label>("Time");
        var startButton = root.Q<Button>("StartBtn");
        
        _gameMode = FindObjectOfType<PinballGameMode>();
        startButton.clicked += () => _gameMode.StartGame();
        _gameMode.UI = this;
    }

    public void SetScore(int score)
    {
        _scoreLabel.text = score.ToString();
    }

    private void Update()
    {
        if (_gameMode.GameStarted)
        {
            float elapsedTime = Time.time - _gameMode.StartTime;
        
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 1000) % 1000);
        
            string formattedTime = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        
            _timeLabel.text = formattedTime; 
        }
        
    }
}
