using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    public StateMachine StateMachine { get; set; }
    private InputBasedMover _inputBasedMover;

    public IdleState(StateMachine stateMachine)
    {
        StateMachine = stateMachine;
        _inputBasedMover = stateMachine.GetComponent<InputBasedMover>();
    }

    public void Enter()
    {
        Debug.Log("Entered idle state");
    }

    public void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float mouseX = Input.GetAxis("Mouse X");
        
        if (horizontal != 0 || vertical != 0 || mouseX != 0) StateMachine.ChangeState(_inputBasedMover.MovingState);
    }

    public void Exit()
    {
        
    }
}

