using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Transactions;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(
    typeof(Rigidbody), 
    typeof(CarVision))]
public class Car : PooledObject, IVehicle, IObstacle
{
    [Header("Car Settings")]
    [SerializeField] float maxAcceleration = 2f;
    [SerializeField] float maxTurnAngle = 10f;
    [SerializeField] float targetDistanceFromObstacle = 3f;
    [SerializeField] float targetTimeFromObstacle = 3f;
    //[SerializeField] float reactionTime = 0.1f;
    [SerializeField] float maxSpeed = 10f;

    /// How fast we should accelerate/brake when not emergency braking
    [SerializeField] float comfortableAcceleration = 0.5f;

    // Wheels
    [SerializeField] private GameObject wheelLF;
    [SerializeField] private GameObject wheelLB;
    [SerializeField] private GameObject wheelRF;
    [SerializeField] private GameObject wheelRB;
    
    // Component references
    private CarVision _carVision;
    private BoxCollider _boxCollider;
    private WheelCollider _frontLeftWheel;
    private WheelCollider _frontRightWheel;
    private WheelCollider _rearLeftWheel;
    private WheelCollider _rearRightWheel;
    
    private Obstacle CurrentObstacle => _carVision.CurrentObstacle;
    
    // Interface implementations
    public Rigidbody Rigidbody { get; private set; }

    public BoxCollider Collider
    {
        get => _boxCollider;
        private set => _boxCollider = value;
    }
    public Vector3 GetBootPosition() => transform.position - transform.forward * 1.225f;
    public Vector3 GetHoodPosition() => transform.position + transform.forward * 1.225f;
    
    private float _currentTurnAngle;
    private float _totalMass;
    private Color _gizmoColor = Color.white;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        _carVision = GetComponent<CarVision>();
        _boxCollider = GetComponent<BoxCollider>();
        Collider = _boxCollider;

        _frontLeftWheel = wheelLF.GetComponent<WheelCollider>();
        _frontRightWheel = wheelRF.GetComponent<WheelCollider>();
        _rearLeftWheel = wheelLB.GetComponent<WheelCollider>();
        _rearRightWheel = wheelRB.GetComponent<WheelCollider>();
    }

    void Start()
    {
        _totalMass = Rigidbody.mass + _frontLeftWheel.mass + _frontRightWheel.mass + _rearLeftWheel.mass + _rearRightWheel.mass;
    }
    
    void Update()
    {
        // Move car
        float accel = Mathf.Clamp(GetTargetAcceleration(), -maxAcceleration, maxAcceleration);
        
        // No backwards acceleration when standing still
        if (Vector3.Dot(transform.forward, Rigidbody.velocity) < 0 && Rigidbody.velocity.magnitude <= 1f) 
            accel = 0;
        
        Rigidbody.AddForce(transform.forward * (accel * _totalMass), ForceMode.Force);
        /*print("Accel: "+accel);
        print("Speed: "+Rigidbody.velocity.magnitude);*/
    }
    
    /// <returns>The accurate distance between this object and obstacle.</returns>
    float GetDistanceToObstacle(GameObject obstacle)
    {
        if (obstacle == null) return float.MaxValue;
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
            float speedDifference = Rigidbody.velocity.magnitude - targetSpeed;
            if (Mathf.Abs(speedDifference) < 0.1f) return 0f; // Avoid jittering
            return speedDifference > 0 ? -comfortableAcceleration : comfortableAcceleration;
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
            if (distanceToObstacle <= targetDistanceFromObstacle)
            {
                // Yes
                print("Full brake");
                return(-maxAcceleration);
            }

            // No ----------------------------------------?
            
            // Are we going to hit it in less than targetTimeFromObstacle?
            if (timeUntilImpact < targetTimeFromObstacle)
            {
                // Yes
                // We have to brake a little
                _gizmoColor = Color.red;
                print("Time brake");
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
            print($"Calculated brake: {requiredAcceleration}");
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
        float s = Vector3.Distance(oPos, tPos) - targetDistanceFromObstacle; // Distance to target
        float a = (v * v - v_0 * v_0) / (2 * s); // Acceleration needed to reach target speed within s
        
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
    
    // Temporary
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
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 2, 0), 0.3f);
        //Gizmos.DrawSphere(_targetDistanceToObstacle, 0.5f);
        //Gizmos.color = Color.magenta;
        //Gizmos.DrawRay(GetComponent<Rigidbody>().centerOfMass, Vector3.up * 5f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetHoodPosition() + new Vector3(0, 0, targetDistanceFromObstacle), 0.5f);
        /*Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(GetHoodPosition(), 0.5f);
        Gizmos.DrawSphere(GetBootPosition(), 0.5f);*/
    }

    float GetSpeedTowardsObstacle()
    {
        float speedTowardsObstacle = Rigidbody.velocity.magnitude;
        var otherRigidBody = CurrentObstacle.rigidBody;
        if (otherRigidBody != null) speedTowardsObstacle -= otherRigidBody.velocity.magnitude;
        return speedTowardsObstacle;
    }
}
