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
    [SerializeField] private float minDistance = 5f;
    
    
    
    private float _currentMotorTorque;
    private float maxMotorTorque = 50f;
    private Vector3 _lastVelocity;
    private bool shouldDecel = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 0.2f;
        rigidbody.velocity = transform.forward * 13.8f;
    }

    void Decelerate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
        // Now that we know the required force, we can apply it to the wheels
        //float brakeTorque = F.magnitude * wheelColl.radius;

        if (shouldDecel)
        {
            // Decelerate
            Vector3 F = rigidbody.mass * GetAcceleration();
            rigidbody.AddForce(-F);
        }
        
        
        _lastVelocity = rigidbody.velocity;
        return;
        var f = CalculateForce();
        rigidbody.AddForce(f);
        print(f);
    
    }

    private Vector3 GetAcceleration()
    {
        return (rigidbody.velocity - _lastVelocity) / Time.fixedDeltaTime;
    }

    private float GetMass()
    {
        return rigidbody.mass;
    }

    private Vector3 CalculateForce()
    {
        // Gather required information for calculating acceleration
        float v_0 = rigidbody.velocity.magnitude;
        float v = 0; //target.GetComponent<Rigidbody>().velocity.magnitude;
        float s = Vector3.Distance(transform.position, target.transform.position);
        float m = rigidbody.mass;
        print(s);
        
        if (s > minDistance)
        {
            // Obey speed limit
            return transform.forward;
        }
        print("Calculating force");
        
        // The target acceleration needed to each the target position and speed
        Vector3 a = (((v * v) - (v_0 * v_0)) / (2 * s)) * transform.forward;
        Vector3 F = m * a;

        return F;
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
    }
    
    private float GetTimeUntilTarget()
    {
        return 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1.5f);
    }
}
