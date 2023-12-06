using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MovingState : IState
{
    public float MoveSpeed;
    public float RotationSpeed;
    public StateMachine StateMachine { get; set; }
    private InputBasedMover _inputBasedMover;

    public MovingState(StateMachine stateMachine, float moveSpeed, float rotationSpeed)
    {
        StateMachine = stateMachine;
        _inputBasedMover = stateMachine.GetComponent<InputBasedMover>();
        MoveSpeed = moveSpeed;
        RotationSpeed = rotationSpeed;
    }

    public void Enter()
    {
        Debug.Log("Entered walking state");
    }

    public void Update()
    {
        // Object movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        if (horizontal == 0 && vertical == 0) StateMachine.ChangeState(_inputBasedMover.IdleState);
        
        var moveSpeed = MoveSpeed;
        var rotSpeed = RotationSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed *= 2;
            rotSpeed *= 2;
        }

        var transform = StateMachine.gameObject.transform;
        
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));

        // Object rotation
        float mouseX = Input.GetAxis("Mouse X");
        Vector3 rotation = new Vector3(0f, mouseX, 0f) * (rotSpeed * Time.deltaTime);
        transform.Rotate(rotation, Space.World);
    }

    public void Exit()
    {
        
    }
    
}
