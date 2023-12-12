using UnityEngine;

public interface IVehicle
{
    /*WheelCollider frontLeftWheel { get; }
    WheelCollider frontRightWheel { get; }
    WheelCollider rearLeftWheel { get; }
    WheelCollider rearRightWheel { get; }*/
    
    public Rigidbody Rigidbody { get; }
    public BoxCollider Collider { get; }
    public Vector3 GetBootPosition();
    public Vector3 GetHoodPosition();
}
