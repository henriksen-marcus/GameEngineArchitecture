using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Transactions;
using UnityEngine;

[RequireComponent(
    typeof(Rigidbody), 
    typeof(WheelController),
    typeof(CarVision))]
[RequireComponent(
    typeof(BoxCollider), // Collision collider
    typeof(BoxCollider), // Vision collider
    typeof(CarAI))]
public class Car : PooledObject, IVehicle, IObstacle
{
    [Header("Car Settings")]
    [SerializeField] float maxMotorTorque = 500f;
    [SerializeField] float maxBrakeTorque = 500f;
    [SerializeField] float maxTurnAngle = 10f;
    [SerializeField] float targetDistanceFromObstacle = 2f;
    [SerializeField] float targetTimeFromObstacle = 2f;
    //[SerializeField] bool takeInput = false;
    //[SerializeField] float reactionTime = 0.1f;
    [SerializeField] float maxSpeed = 3f;

    /// How fast we should accelerate/brake when not emergency braking
    [SerializeField] float comfortableAcceleration = 0.5f;
    
    private CarVision _carVision;
    private Obstacle CurrentObstacle => _carVision.CurrentObstacle;
    private WheelController _wheelController;
    
    // TODO: Check
    private float MaxDeceleration => maxBrakeTorque / Rigidbody.mass;
    
    [Header("Wheel Colliders")]
    // Properties are not settable in the inspector
    [SerializeField] public WheelCollider frontLeftWheel;
    [SerializeField] public WheelCollider frontRightWheel;
    [SerializeField] public WheelCollider rearLeftWheel;
    [SerializeField] public WheelCollider rearRightWheel;
    
    // Interface implementations
    public Rigidbody Rigidbody { get; set; }
    public BoxCollider Collider { get; private set; }
    public Vector3 GetBootPosition() => transform.position - transform.forward * Collider.size.z / 2;
    public Vector3 GetHoodPosition() => transform.position + transform.forward * Collider.size.z / 2;

    private float _currentMotorTorque;
    private float _currentBrakeTorque;
    private float _currentTurnAngle;
    private Color _gizmoColor = Color.white;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<BoxCollider>();
        _carVision = GetComponent<CarVision>();
        _wheelController = GetComponent<WheelController>();
        frontLeftWheel.brakeTorque = 0;
        frontRightWheel.brakeTorque = 0;
        rearLeftWheel.brakeTorque = 0;
        rearRightWheel.brakeTorque = 0;
    }
    
    void Update()
    {
        /*var coll = Physics.OverlapBox(transform.position + _boxOverlapPosition, _boxOverlapHalfSize);
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
        else if (isActive) Move();*/
        if (Input.GetKey(KeyCode.Space))
        {
            print("Force");
            Rigidbody.AddForce(transform.forward * 2000f);
        }
    }
    
    private void PlayerMove()
    {
        return;
        _currentMotorTorque = Input.GetAxisRaw("Vertical") * maxMotorTorque;
        if (Rigidbody.velocity.magnitude > maxSpeed) _currentMotorTorque = 0;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            _currentBrakeTorque = maxBrakeTorque;
        }
        else
        {
            _currentBrakeTorque = 0;
        }
        
        _wheelController.Accelerate(_currentMotorTorque);
        _wheelController.Brake(_currentBrakeTorque);
        
        _currentTurnAngle = maxTurnAngle * Input.GetAxisRaw("Horizontal");
        frontLeftWheel.steerAngle = _currentTurnAngle;
        frontRightWheel.steerAngle = _currentTurnAngle;
    }

    private void FixedUpdate()
    {
        
    }

    private float ForceToTorque(float force)
    {
        return force * frontRightWheel.radius;
    }
    
    float GetDistanceToObstacle(GameObject obstacle)
    {
        switch (obstacle.tag)
        {
            case "Car": // If the obstacle is a car, we can get a more precise distance
                var otherCar = obstacle.GetComponent<Car>();
                if (otherCar != null) return Vector3.Distance(GetHoodPosition(), otherCar.GetBootPosition());
                return -1;
            default:
                return Vector3.Distance(GetHoodPosition(), obstacle.transform.position);
        }
    }

    private float GetTargetAcceleration()
    {
        float speedTowardsObstacle = GetSpeedTowardsObstacle();
        float distanceToObstacle = GetDistanceToObstacle(CurrentObstacle.gameObject);
        float distanceToBrakePosition = distanceToObstacle - targetDistanceFromObstacle;
        float timeUntilImpact = distanceToBrakePosition / speedTowardsObstacle;
        
        // 1. Is there an obstacle ahead?
        if (CurrentObstacle.gameObject != null)
        {
            // Yes
            _gizmoColor = Color.yellow;
            // 2. Are we very close to the obstacle, less than targetDistanceFromObstacle?
            if (distanceToObstacle <= targetDistanceFromObstacle)
            {
                // Yes
                
            }
            else
            {
                // No
                
                // Accelerate
                _gizmoColor = Color.green;
                _currentMotorTorque = maxMotorTorque;
                _currentBrakeTorque = 0;
            }
            
            // 3. Are we going to hit it in less than targetTimeFromObstacle?
            _gizmoColor = Color.white;
            float targetSpeed = CurrentObstacle.rigidBody ? CurrentObstacle.rigidBody.velocity.magnitude : 0;
            float speedDifference = targetSpeed - Rigidbody.velocity.magnitude;
            // Find acceleration/deceleration
        }
        else
        {
            // No
            float targetSpeed = GetSpeedLimit();
            float speedDifference = targetSpeed - Rigidbody.velocity.magnitude;
            if (Mathf.Abs(speedDifference) < 1f) return 0f; // Avoid jittering
            return speedDifference > 0 ? comfortableAcceleration : -comfortableAcceleration;
        }

        return 0;
    }
    
    private float GetSpeedLimit() => maxSpeed; 

    public void Move(Vector2 direction)
    {
        _currentMotorTorque = maxMotorTorque;
        _currentBrakeTorque = 0;
        
        if (CurrentObstacle.gameObject != null)
        {
            _gizmoColor = Color.yellow;
            
            Vector3 ourPos = transform.position;
            Vector3 otherPos = CurrentObstacle.gameObject.transform.position;
            
            float speedTowardsObstacle = GetSpeedTowardsObstacle();
            float distanceToObstacle = GetDistanceToObstacle(CurrentObstacle.gameObject);
            float distanceToBrakePosition = distanceToObstacle - targetDistanceFromObstacle;
            float timeUntilImpact = distanceToBrakePosition / speedTowardsObstacle;

            // Brake if:
            // 1. We are closer than the minimum distance, aka targetDistanceFromObstacle
            // 2. We are going to hit the obstacle in less than targetTimeFromObstacle

            if (distanceToObstacle <= targetDistanceFromObstacle)
            {
                if (speedTowardsObstacle >= 0) // Brake
                {
                    _gizmoColor = Color.red;
                    //float brakeCoefficient = Mathf.Clamp(Vector3.Distance(ourPos, otherPos) / targetDistanceFromObstacle, 0, 1);
                    Rigidbody.velocity = Vector3.zero;
                }
                else // Accelerate
                {
                    _gizmoColor = Color.green;
                    _currentMotorTorque = maxMotorTorque;
                    _currentBrakeTorque = 0;
                }
            }
            else if (timeUntilImpact <= targetTimeFromObstacle)
            {
                // Brake based on the distance and time until impact
                _gizmoColor = Color.red;
                //float brakeCoefficient = Mathf.Clamp(1 - (timeUntilImpact / targetTimeFromObstacle), 0f, 1f);
                //_carMovement.Brake(maxBrakeTorque * brakeCoefficient);
            }
            
            
        }
        else
        {
            _gizmoColor = Color.white;
        }
        
        /*StartCoroutine(WaitAccelerate(_currentMotorTorque));
        StartCoroutine(WaitBrake(_currentBrakeTorque));*/
    }
    

    /*IEnumerator WaitAccelerate(float torque)
    {
        yield return new WaitForSeconds(reactionTime);
        Accelerate(torque);
    }
    
    IEnumerator WaitBrake(float torque)
    {
        yield return new WaitForSeconds(reactionTime);
        
        Brake(torque);
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        //Gizmos.DrawWireCube(transform.position + _boxOverlapPosition, _boxOverlapHalfSize * 2);
        //Gizmos.DrawSphere(_targetDistanceToObstacle, 0.5f);
        //Gizmos.color = Color.magenta;
        //Gizmos.DrawRay(GetComponent<Rigidbody>().centerOfMass, Vector3.up * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        
    }

    float GetSpeedTowardsObstacle()
    {
        float speedTowardsObstacle = Rigidbody.velocity.magnitude;
        var otherRigidBody = _carVision.CurrentObstacle.rigidBody;
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
        Vector3 otherPos = _carVision.CurrentObstacle.gameObject.transform.position;
        return otherPos + (transform.position - otherPos).normalized * targetDistanceFromObstacle;
    }
}
