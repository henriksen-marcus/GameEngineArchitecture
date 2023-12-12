using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarOrder : MonoBehaviour
{
    [SerializeField] private WheelCollider _frontLeftWheel;
    [SerializeField] private WheelCollider _frontRightWheel;

    public float maxTurnAngle = 45f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Raycast from camera position
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 100f))
        {
            SteerToPosition(hit.point);
        }
    }
    
    public void SteerToPosition(Vector3 targetPosition)
    {
        // Calculate the direction towards the target position
        Vector3 targetDirection = targetPosition - transform.position;

        // Normalize the direction to avoid exceeding the max steering angle
        targetDirection.Normalize();

        // Calculate the angle between the car's forward direction and the target direction
        float angle = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);

        // Scale the angle to the max steering angle
        float steeringAngle = Mathf.Clamp(angle * maxTurnAngle / 90f, -maxTurnAngle, maxTurnAngle);
        
        _frontLeftWheel.steerAngle = steeringAngle;
        _frontRightWheel.steerAngle = steeringAngle;
    }
}
