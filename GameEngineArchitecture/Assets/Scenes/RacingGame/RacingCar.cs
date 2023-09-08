using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingCar : MonoBehaviour
{
    private int Health = 100;
    private Rigidbody m_rigidbody;
    [SerializeField] [Range(0f, 100f)] float force = 10f;

    Vector3 InitialPos;
     Quaternion InitialRot;

     static float tolerance = 2f;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.maxAngularVelocity = .5f;
        InitialPos = transform.position;
        InitialRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            m_rigidbody.AddForce(transform.forward * force);

        if (Input.GetKey(KeyCode.S))
            m_rigidbody.AddForce(-transform.forward * force);

        if (Input.GetKey(KeyCode.A))
            transform.Rotate(0, -0.5f, 0);

        if (Input.GetKey(KeyCode.D))
            transform.Rotate(0, 0.5f, 0);


        if (transform.position.y < -8)
        {
            Reset();
        }
    }

    void Reset()
    {
        transform.position = InitialPos;
        transform.rotation = InitialRot;

        m_rigidbody.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.name.Contains("DropBox")) 
        {
            Health -= 10;
            print($"Collided with {other.gameObject.name}, health is now {Health}");
            Destroy(other.gameObject);
        }
    }
}
