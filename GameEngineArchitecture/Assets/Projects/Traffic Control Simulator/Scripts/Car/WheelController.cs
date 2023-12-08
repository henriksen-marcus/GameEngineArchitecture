using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller for a car with four wheels.
/// </summary>
[RequireComponent(typeof(Car))]
public class WheelController : MonoBehaviour
{
    private Car _car;

    private void Awake()
    {
        _car = GetComponent<Car>();
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private float ForceToTorque(float force)
    {
        return force * _car.frontRightWheel.radius;
    }

    public void Move(Vector2 direction)
    {
        
    }
    
    public void Accelerate(float force)
    {
        var torque = ForceToTorque(force);
        _car.frontRightWheel.motorTorque = torque;
        _car.frontLeftWheel.motorTorque = torque;
    }
    
    public void Brake(float force)
    {
        var torque = ForceToTorque(force);
        _car.frontRightWheel.brakeTorque = torque;
        _car.frontLeftWheel.brakeTorque = torque;
        _car.rearRightWheel.brakeTorque = torque;
        _car.rearLeftWheel.brakeTorque = torque;
    }
}
