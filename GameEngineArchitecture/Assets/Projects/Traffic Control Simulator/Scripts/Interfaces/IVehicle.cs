using UnityEngine;

public interface IVehicle
{
    public Rigidbody Rigidbody { get; }
    public BoxCollider Collider { get; }
    public Vector3 GetBootPosition();
    public Vector3 GetHoodPosition();
}
