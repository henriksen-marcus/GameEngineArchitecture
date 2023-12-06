using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// This script handles movement, input and other actions for the player.
/// </summary>
public class InputBasedMover : StateMachine
{
    [FormerlySerializedAs("_moveSpeed")] [SerializeField] protected float moveSpeed = 3f;
    [FormerlySerializedAs("_rotationSpeed")] [SerializeField] protected float rotationSpeed = 3f;

    public IdleState IdleState;
    public MovingState MovingState;

    private void Start()
    {
        IdleState = new(this);
        MovingState = new(this, moveSpeed, rotationSpeed);
  
        Initialize(IdleState);
        
        // Hide the cursor and lock it to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        // Make the cursor invisible
        Cursor.visible = false;
    }
}
