using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public interface IState
{
    public StateMachine StateMachine { get; set; }
    
    // From C# 8.0 onwards, interfaces can have default implementations.
    void Enter();
    void Update();
    void Exit();
}
