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
    [SerializeField] bool isActive = true;
    [SerializeField] bool takeInput = false;
    [SerializeField] float reactionTime = 0.1f;

    private float _currentMotorTorque;
    private float _currentBrakeTorque;
    private float _currentTurnAngle;
    private Color _gizmoColor = Color.white;
    private float _targetDistanceToObstacle;
    
    private Vector3 _boxOverlapPosition = new Vector3(0, 2.13f, 7.85f);
    private Vector3 _boxOverlapHalfSize = new Vector3(0.75f, 2.145f, 6.5f);
    
    private Obstacle _currentObstacle = new Obstacle(null);

    public Rigidbody rigidBody;
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        
    }
    
    void Update()
    {
        var coll = Physics.OverlapBox(transform.position + _boxOverlapPosition, _boxOverlapHalfSize);
        try
        {
            var other = coll[0];
            if (other.CompareTag("Car"))
            {
                _currentObstacle = new Obstacle(other.gameObject);
               // print("ENTER");
               
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
                    float c1 = Mathf.Clamp(timeUntilImpact / targetTimeFromObstacle, 0, 1);
                    float c2 = Mathf.Clamp(Vector3.Distance(ourPos, otherPos) / targetDistanceFromObstacle, 0, 1);
                    
                    print(Mathf.Max(c1, c2));
                    //print("C1: " + Mathf.Clamp(timeUntilImpact / targetTimeFromObstacle, 0, 1) + " C2: " + Mathf.Clamp(targetDistanceFromObstacle / Vector3.Distance(ourPos, otherPos), 0, 1));
                    //_currentBrakeTorque = maxBrakeTorque/*Mathf.Lerp(0, maxBrakeTorque, timeUntilImpact / targetTimeFromObstacle)*/;
                    _currentBrakeTorque = Mathf.Lerp(0, maxBrakeTorque, c2);
                    _currentMotorTorque = 0;
                }
            }
        }
        else
        {
            _gizmoColor = Color.white;
        }
        
        StartCoroutine(WaitAccelerate(_currentMotorTorque));
        StartCoroutine(WaitBrake(_currentBrakeTorque));
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

    IEnumerator WaitAccelerate(float torque)
    {
        yield return new WaitForSeconds(reactionTime);
        Accelerate(torque);
    }
    
    IEnumerator WaitBrake(float torque)
    {
        yield return new WaitForSeconds(reactionTime);
        
        Brake(torque);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireCube(transform.position + _boxOverlapPosition, _boxOverlapHalfSize * 2);
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
        
        _currentMotorTorque = 0;
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
