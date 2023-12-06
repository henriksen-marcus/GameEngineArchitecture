using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Base class for any class based on states.
/// </summary>
[Serializable]
public abstract class StateMachine : MonoBehaviour
{
    public IState CurrentState { get; protected set; }
    
    public virtual void Initialize(IState initialState)
    {
        CurrentState = initialState;
        CurrentState?.Enter();
    }
    
    public virtual void ChangeState(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }
    
    void Update()
    {
        CurrentState?.Update();
    }
}
