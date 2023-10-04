using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] WheelCollider frontLeftWheel;
    [SerializeField] WheelCollider frontRightWheel;
    [SerializeField] WheelCollider rearLeftWheel;
    [SerializeField] WheelCollider rearRightWheel;
    [SerializeField] float maxMotorForce = 500f;
    [SerializeField] float maxBrakeTorque = 500f;
    [SerializeField] float maxTurnAngle = 10f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] private bool isActive = true;
    
    private float _currentMotorForce;
    private float _currentBrakeTorque;
    private float _currentTurnAngle;

    private Rigidbody _rb;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) Move();
    }

    private void FixedUpdate()
    {
        
    }

    void Move()
    {
        _currentMotorForce = maxMotorForce * Input.GetAxisRaw("Vertical");
        if (_rb.velocity.magnitude > maxSpeed) _currentMotorForce = 0;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            _currentBrakeTorque = maxBrakeTorque;
        }
        else
        {
            _currentBrakeTorque = 0;
        }

        frontRightWheel.motorTorque = _currentMotorForce;
        frontLeftWheel.motorTorque = _currentMotorForce;
        
        frontRightWheel.brakeTorque = _currentBrakeTorque;
        frontLeftWheel.brakeTorque = _currentBrakeTorque;
        rearRightWheel.brakeTorque = _currentBrakeTorque;
        rearLeftWheel.brakeTorque = _currentBrakeTorque;
        
        _currentTurnAngle = maxTurnAngle * Input.GetAxisRaw("Horizontal");
        frontLeftWheel.steerAngle = _currentTurnAngle;
        frontRightWheel.steerAngle = _currentTurnAngle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            print("Seeing car");
        }
        else if (other.CompareTag("TrafficLight"))
        {
            print("Seeing traffic light");
        }
    }
}
