using UnityEngine;

/// <summary>
/// Data container for car obstacles.
/// </summary>
public struct Obstacle
{
    public GameObject gameObject;
    public Rigidbody rigidBody;
    public Vector3 hitPosition;

    public Obstacle(GameObject other, Vector3 hitPosition)
    {
        gameObject = other;
        rigidBody = other?.GetComponent<Rigidbody>();
        this.hitPosition = hitPosition;
    }
}
