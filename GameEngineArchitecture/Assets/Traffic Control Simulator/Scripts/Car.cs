using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Car : MonoBehaviour
{
    public struct Obstacle
    {
        public GameObject gameObject;
        public Rigidbody rigidBody;

        public Obstacle(GameObject other)
        {
            gameObject = other;
            rigidBody = other?.GetComponent<Rigidbody>();
        }
    }
    
    [SerializeField] WheelCollider frontLeftWheel;
    [SerializeField] WheelCollider frontRightWheel;
    [SerializeField] WheelCollider rearLeftWheel;
    [SerializeField] WheelCollider rearRightWheel;
    [SerializeField] BoxCollider visionCollider;
    [SerializeField] float maxMotorTorque = 500f;
    [SerializeField] float maxBrakeTorque = 500f;
    [SerializeField] float maxTurnAngle = 10f;
    [SerializeField] float maxSpeed = 3f;
    [SerializeField] float targetDistanceFromObstacle = 2f;
    [SerializeField] float targetTimeFromObstacle = 2f;
    [SerializeField] private bool isActive = true;
    [SerializeField] private bool takeInput = false;

    private float _currentMotorTorque;
    private float _currentBrakeTorque;
    private float _currentTurnAngle;
    private Color _gizmoColor = Color.white;
    private float _targetDistanceToObstacle;
    
    private Obstacle _currentObstacle = new Obstacle(null);

    public Rigidbody rigidBody;
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        var coll = Physics.OverlapBox(transform.position + new Vector3(0, 2.13f, 5.4f), new Vector3(0.75f, 2.145f, 4.13f));
        try
        {
            var other = coll[0];
            if (other.CompareTag("Car"))
            {
                _currentObstacle = new Obstacle(other.gameObject);
                print("ENTER");
            }
            else if (other.CompareTag("TrafficLight"))
            {
                print("Seeing traffic light");
            }
            else
            {
                _currentObstacle = new Obstacle(null);
            }
        }
        catch
        {
            _currentObstacle = new Obstacle(null);
        }

        if (takeInput) PlayerMove();
        else if (isActive) Move();
    }

    private void PlayerMove()
    {
        _currentMotorTorque = Input.GetAxisRaw("Vertical") * maxMotorTorque;
        if (rigidBody.velocity.magnitude > maxSpeed) _currentMotorTorque = 0;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            _currentBrakeTorque = maxBrakeTorque;
        }
        else
        {
            _currentBrakeTorque = 0;
        }
        
        Accelerate(_currentMotorTorque);
        Brake(_currentBrakeTorque);
        
        _currentTurnAngle = maxTurnAngle * Input.GetAxisRaw("Horizontal");
        frontLeftWheel.steerAngle = _currentTurnAngle;
        frontRightWheel.steerAngle = _currentTurnAngle;
    }

    private void FixedUpdate()
    {
        
    }

    void Move()
    {
        _currentMotorTorque = maxMotorTorque;
        _currentBrakeTorque = 0;
        
        if (_currentObstacle.gameObject)
        {
            _gizmoColor = Color.yellow;
            
            float speedTowardsObstacle = GetSpeedTowardsObstacle();
            
            // Calculate minimum distance from the obstacle
            Vector3 ourPos = transform.position;
            Vector3 otherPos = _currentObstacle.gameObject.transform.position;
            
            Vector3 brakePosition = GetBrakePosition();
            
            // Calculate target distance from the obstacle with a time buffer
            
            float distanceToObstacle = Vector3.Distance(ourPos, otherPos);
            float targetDistanceInTime = distanceToObstacle / speedTowardsObstacle;
                
            float distanceToBrakePosition = Vector3.Distance(ourPos, otherPos);
            float timeUntilImpact = distanceToBrakePosition / speedTowardsObstacle;

            //_targetDistanceToObstacle = brakePosition;
            
            //print(timeUntilImpact / targetTimeFromObstacle);
            
            //bool farEnoughInTime = timeUntilImpact > targetTimeFromObstacle && otherRigidBody.velocity.magnitude > 0.5f;

            // Brake if..
            if (Vector3.Distance(ourPos, otherPos) <= targetDistanceFromObstacle || (timeUntilImpact <= targetTimeFromObstacle))
            {
                if (speedTowardsObstacle >= 0)
                {
                    _gizmoColor = Color.red;
                    _currentBrakeTorque = maxBrakeTorque/*Mathf.Lerp(0, maxBrakeTorque, timeUntilImpact / targetTimeFromObstacle)*/;
                    _currentMotorTorque = 0;
                }
            }
            
        }
        else
        {
            _gizmoColor = Color.white;
        }
        
        Accelerate(_currentMotorTorque);
        Brake(_currentBrakeTorque);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            _currentObstacle = new Obstacle(other.gameObject);
            print("ENTER");
        }
        else if (other.CompareTag("TrafficLight"))
        {
            print("Seeing traffic light");
        }
    }*/

    private void OnTriggerStay(Collider other)
    {
        /*if (other.CompareTag("Car"))
        {
            _currentObstacle = new Obstacle(other.gameObject);
            print("Seeing car");
        }
        else if (other.CompareTag("TrafficLight"))
        {
            print("Seeing traffic light");
        }*/
    }

    /*private void OnTriggerExit(Collider other)
    {
        print("EXIT");
        _currentObstacle = new Obstacle(null);
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, 2.13f, 5.4f), new Vector3(0.75f*2, 2.145f*2, 4.13f*2));
        //Gizmos.DrawSphere(_targetDistanceToObstacle, 0.5f);
    }

    void Accelerate(float torque)
    {
        frontRightWheel.motorTorque = torque;
        frontLeftWheel.motorTorque = torque;
    }
    
    void Brake(float torque)
    {
        frontRightWheel.brakeTorque = torque;
        frontLeftWheel.brakeTorque = torque;
        rearRightWheel.brakeTorque = torque;
        rearLeftWheel.brakeTorque = torque;
        
        // Light up brake lights
    }
    
    float GetSpeedTowardsObstacle()
    {
        float speedTowardsObstacle = rigidBody.velocity.magnitude;
        var otherRigidBody = _currentObstacle.rigidBody;
        if (otherRigidBody) speedTowardsObstacle -= otherRigidBody.velocity.magnitude;
        return speedTowardsObstacle;
    }
    
    /// <summary>
    /// Get the closest position behind the obstacle in front of
    /// us we can be according targetDistanceFromObstacle
    /// </summary>
    /// <returns></returns>
    Vector3 GetBrakePosition()
    {
        Vector3 otherPos = _currentObstacle.gameObject.transform.position;
        return otherPos + (transform.position - otherPos).normalized * targetDistanceFromObstacle;
    }
}
