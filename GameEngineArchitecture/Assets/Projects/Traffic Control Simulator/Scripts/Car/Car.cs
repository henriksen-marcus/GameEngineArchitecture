using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Transactions;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(
    typeof(Rigidbody), 
    typeof(WheelController),
    typeof(CarVision))]
public class Car : PooledObject, IVehicle, IObstacle
{
    [FormerlySerializedAs("maxMotorTorque")]
    [Header("Car Settings")]
    [SerializeField] float maxAcceleration = 1f;
    [SerializeField] float maxTurnAngle = 10f;
    [SerializeField] float targetDistanceFromObstacle = 2f;
    [SerializeField] float targetTimeFromObstacle = 2f;
    //[SerializeField] bool takeInput = false;
    //[SerializeField] float reactionTime = 0.1f;
    [SerializeField] float maxSpeed = 3f;
    [SerializeField] private BoxCollider boxCollider;

    /// How fast we should accelerate/brake when not emergency braking
    [SerializeField] float comfortableAcceleration = 0.5f;
    
    private Obstacle CurrentObstacle => _carVision.CurrentObstacle;
    
    // Component references
    private CarVision _carVision;
    private WheelController _wheelController;
    private WheelCollider _frontLeftWheel;
    private WheelCollider _frontRightWheel;
    private WheelCollider _rearLeftWheel;
    private WheelCollider _rearRightWheel;
    
    // Interface implementations
    public Rigidbody Rigidbody { get; set; }
    public BoxCollider Collider { get; private set; }
    public Vector3 GetBootPosition() => transform.position - transform.forward * Collider.size.z / 2;
    public Vector3 GetHoodPosition() => transform.position + transform.forward * Collider.size.z / 2;
    
    private float _currentTurnAngle;
    private float _totalMass;
    private Color _gizmoColor = Color.white;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<BoxCollider>();
        _carVision = GetComponent<CarVision>();
        _wheelController = GetComponent<WheelController>();
        
        _totalMass = Rigidbody.mass + _frontLeftWheel.mass + _frontRightWheel.mass + _rearLeftWheel.mass + _rearRightWheel.mass;
    }
    
    void Update()
    {
        // Move car
        Rigidbody.AddForce(transform.forward * (GetTargetAcceleration() * _totalMass), ForceMode.Force);
    }

    private void FixedUpdate()
    {
        
    }

    private float ForceToTorque(float force)
    {
        return force * _frontRightWheel.radius;
    }
    
    /// <returns>The accurate distance between this object and obstacle.</returns>
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
        float AdjustToSpeedLimit()
        {
            _gizmoColor = Color.green;
            float targetSpeed = GetSpeedLimit();
            float speedDifference = targetSpeed - Rigidbody.velocity.magnitude;
            if (Mathf.Abs(speedDifference) < 1f) return 0f; // Avoid jittering
            return speedDifference > 0 ? comfortableAcceleration : -comfortableAcceleration;
        }
        
        float speedTowardsObstacle = GetSpeedTowardsObstacle();
        float distanceToObstacle = GetDistanceToObstacle(CurrentObstacle.gameObject);
        float distanceToBrakePosition = distanceToObstacle - targetDistanceFromObstacle;
        float timeUntilImpact = distanceToBrakePosition / speedTowardsObstacle;
        
        // Is there an obstacle ahead?
        if (CurrentObstacle.gameObject != null)
        {
            // Yes
            _gizmoColor = Color.yellow;
            // Are we very close to the obstacle, less than targetDistanceFromObstacle?
            if (distanceToObstacle < targetDistanceFromObstacle)
            {
                // Yes
                return(-maxAcceleration);
            }

            // No ----------------------------------------?
            
            // Are we going to hit it in less than targetTimeFromObstacle?
            if (timeUntilImpact < targetTimeFromObstacle)
            {
                // Yes
                // We have to brake a little
                _gizmoColor = Color.red;
                return (-comfortableAcceleration);
            } 
            
            // No
            // Calculate deceleration from our position to obstacle
            float requiredAcceleration = CalculateAccelerationTo(CurrentObstacle);
            
            // Is accel below the threshold for comfortable acceleration?
            if (Mathf.Abs(requiredAcceleration) < comfortableAcceleration)
            {
                // Yes
                // We don't have to brake yet, so adjust to speed limit
                return AdjustToSpeedLimit();
            }
            
            // No
            // We have to brake
            _gizmoColor = Color.red;
            return requiredAcceleration;
        }
 
        // No
        // Find speed limit and adjust to it
        return AdjustToSpeedLimit();
    }
    
    private float CalculateAccelerationTo(Obstacle obstacle)
    {
        // Gather required information for calculating acceleration
        //Vector3 velocity = Rigidbody.velocity;
        Vector3 oPos = transform.position; // Our
        Vector3 tPos = obstacle.gameObject.transform.position; // Target
        float m = _totalMass;
        
        float oVelocity = Rigidbody.velocity.magnitude;
        float tVelocity = obstacle.rigidBody ? obstacle.rigidBody.velocity.magnitude : 0;
        
        // Calculate acceleration by using the kinematic equation for each vector component
        
        // We only really need the one-dimensional acceleration because we only accelerate in the forward direction anyways
        float v_0 = oVelocity; // Starting speed
        float v = tVelocity; // Target speed
        float s = MathF.Abs(oPos.x - tPos.x); // Distance to target
        float a = s == 0f ? 0f : (v * v - v_0 * v_0) / (2 * s); // Acceleration needed to reach target speed within s
        
        /*float v_0_x = velocity.x; // Starting speed
        float v_x = 0; // Target speed
        float s_x = MathF.Abs(oPos.x - tPos.x); // Distance to target
        float a_x = s_x == 0f ? 0f : (v_x * v_x - v_0_x * v_0_x) / (2 * s_x); // Acceleration needed to reach target speed within s
        
        float v_0_y = velocity.y;
        float v_y = 0;
        float s_y = MathF.Abs(oPos.y - tPos.y);
        float a_y = s_y == 0f ? 0f : (v_y * v_y - v_0_y * v_0_y) / (2 * s_y);
        
        float v_0_z = velocity.z;
        float v_z = 0;
        float s_z = MathF.Abs(oPos.z - tPos.z);
        float a_z = s_z == 0f ? 0f : (v_z * v_z - v_0_z * v_0_z) / (2 * s_z);*/
        
        // The target acceleration needed to each the target position and speed
        //Vector3 a = new Vector3(a_x, a_y, a_z);
        
        /*print("v_x: " + v_x + " v_0_x: " + v_0_x + " s_x: " + s_x + " a_x: " + a_x);
        print("v_y: " + v_y + " v_0_y: " + v_0_y + " s_y: " + s_y + " a_y: " + a_y);
        print("v_z: " + v_z + " v_0_z: " + v_0_z + " s_z: " + s_z + " a_z: " + a_z);
        print(a);*/
        
        //return a.magnitude;
        return a;
    }
    
    private float GetSpeedLimit() => maxSpeed; 
    
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
        Gizmos.DrawWireCube(transform.position, boxCollider.size);
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
        var otherRigidBody = CurrentObstacle.rigidBody;
        if (otherRigidBody != null) speedTowardsObstacle -= otherRigidBody.velocity.magnitude;
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
