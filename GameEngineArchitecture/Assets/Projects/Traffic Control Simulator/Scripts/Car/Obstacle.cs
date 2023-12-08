using UnityEngine;

/// <summary>
/// Data container for car obstacles.
/// </summary>
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
