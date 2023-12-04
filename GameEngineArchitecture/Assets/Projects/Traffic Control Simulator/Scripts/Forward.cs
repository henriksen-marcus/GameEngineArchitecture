using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Forward : MonoBehaviour
{
    
    [SerializeField] private WheelCollider wheelColl;
    [SerializeField] private GameObject target;
    [SerializeField] private Rigidbody rigidbody;
    /// <summary>
    /// The minimum distance from an obstacle ahead before we start
    /// emergency braking.
    /// </summary>
    [SerializeField] private float minDistance = 5f;
    /// <summary>
    /// The minimum distance we want to keep from the car in front of us.
    /// Should be higher than minDistance. This value will be overriden
    /// to be higher when travelling at high speeds.
    /// </summary>
    [SerializeField] private float followDistance = 10f;
    /// <summary>
    /// The max braking acceleration we can apply to the car, for
    /// example when emergency braking.
    /// </summary>
    [SerializeField] private float maxBrakeAcceleration = 5f;

    [SerializeField] private float comfyAccelerationThreshold = 3f;
    
    /// <summary>
    /// The velocity of the car in the last FixedUpdate.
    /// </summary>
    private Vector3 _lastVelocity;
    private bool shouldDecel = true;
    private float _stillThreshold = 0.1f;
    
    void Start()
    {
        //Time.timeScale = 0.2f;
        rigidbody.velocity = transform.forward * 10f;
        print(GetTargetAcceleration());
        
    }
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            Vector3 F = rigidbody.mass * GetAcceleration();
            rigidbody.AddForce(-F);
        }
    }

    private void FixedUpdate()
    {
        Vector3 F = Vector3.zero;
        
        var targetRB = target.GetComponent<Rigidbody>();
        if (targetRB)
        {
            if (targetRB.velocity.magnitude <= _stillThreshold)
            {
                // Calculate deceleration from our position to obstacle
                F = GetTargetAcceleration();
                // We don't have to brake yet, so adjust to speed limit
                if (F.magnitude <= comfyAccelerationThreshold)
                {
                    // Adjust to speed limit
                }
                else // We have to brake
                {
                    rigidbody.AddForce(F, ForceMode.Acceleration);
                }
            }
        }

        // Are we very close to the obstacle?
        if (Vector3.Distance(transform.position, target.transform.position) < minDistance)
        {
            // Max brake
            F = -rigidbody.velocity.normalized * maxBrakeAcceleration;
            rigidbody.AddForce(F, ForceMode.Acceleration);
            return;
        }
        
        F = GetTargetAcceleration();
        // Decelerate, since we use ForceMode.Acceleration, we can just say F = a
        rigidbody.AddForce(F, ForceMode.Acceleration);
        
        
        // We also need to know how far our brake distance is, take into account their braking distance as well!

        // Check if we should accelerate or decelerate
        /*if (v_0 <= target.GetComponent<Rigidbody>().velocity.magnitude)
        {
        }
        else
        {
        }*/

        // We have found out the required force, now check if we need to apply it yet.
        // use SVT to calculate the time it takes to the target at our and their current velocity,
        // and check if this is less than 3 seconds (or the time set by the player)
        
        /*if (s > minDistance)
        {
            // Obey speed limit
            return transform.forward;
        }
        print("Calculating force");*/
        
        // Now that we know the required force, we can apply it to the wheels
        //float brakeTorque = F.magnitude * wheelColl.radius;
        
        _lastVelocity = rigidbody.velocity;
    }

    private Vector3 GetAcceleration()
    {
        return (rigidbody.velocity - _lastVelocity) / Time.fixedDeltaTime;
    }

    private float GetMass() => rigidbody.mass;

    /// <returns>The closest position behind the car in front of us that we want to be.</returns>
    private Vector3 GetFollowPosition()
    {
        var oPos = transform.position;
        var tPos = target.transform.position;
        return tPos - (oPos + tPos).normalized * followDistance;
    }

    private Vector3 GetTargetAcceleration()
    {
        // Gather required information for calculating acceleration
        Vector3 velocity = rigidbody.velocity;
        Vector3 oPos = transform.position; // Our
        Vector3 tPos = GetFollowPosition(); // Target
        float m = rigidbody.mass;
        
        // Calculate acceleration by using the kinematic equation for each vector component
        
        float v_0_x = velocity.x; // Starting speed
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
        float a_z = s_z == 0f ? 0f : (v_z * v_z - v_0_z * v_0_z) / (2 * s_z);
        
        // The target acceleration needed to each the target position and speed
        Vector3 a = new Vector3(a_x, a_y, a_z);
        
        /*print("v_x: " + v_x + " v_0_x: " + v_0_x + " s_x: " + s_x + " a_x: " + a_x);
        print("v_y: " + v_y + " v_0_y: " + v_0_y + " s_y: " + s_y + " a_y: " + a_y);
        print("v_z: " + v_z + " v_0_z: " + v_0_z + " s_z: " + s_z + " a_z: " + a_z);
        print(a);*/
        
        return a;
    }
    
    private float GetTimeUntilTarget()
    {
        return 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1.5f);
        Gizmos.DrawSphere(GetFollowPosition(), 1);
    }
}
