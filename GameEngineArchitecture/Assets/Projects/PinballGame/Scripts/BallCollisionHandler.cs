using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BallCollisionHandler : MonoBehaviour
{
    [SerializeField] float triggerDelay = 1f;
    protected Collider _other;
    private PinballGameMode _gameMode;
    public abstract int scoreAmount { get; set; }

    private void Start()
    {
        _gameMode = FindObjectOfType<PinballGameMode>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Ball"))
        {
            _other = other;
            Invoke(nameof(HandleTriggerEnter), triggerDelay);
            _gameMode.AddScore(scoreAmount);
        }
    }
    
    protected abstract void HandleTriggerEnter();
}
