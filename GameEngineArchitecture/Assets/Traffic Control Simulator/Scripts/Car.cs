using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] WheelCollider frontLeftWheel;
    [SerializeField] WheelCollider frontRightWheel;
    [SerializeField] WheelCollider rearLeftWheel;
    [SerializeField] WheelCollider rearRightWheel;
    [SerializeField] float maxMotorForce = 1500f;
    [SerializeField] float maxBrakeTorque = 100f;
    [SerializeField] float maxSteerAngle = 30f;
    
    private float _currentMotorForce;
    private float _currentBrakeTorque;

    private Rigidbody _rb;
    
    void Start()
    {
        _rb = transform.Find("body").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        _currentMotorForce = maxMotorForce * Input.GetAxis("Vertical");

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
    }
}
