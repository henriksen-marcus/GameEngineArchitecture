using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingCube : MonoBehaviour
{

    public Rigidbody rigidbody;
    [SerializeField] [Range(0f, 100f)] float force = 10f;

    Vector3 InitialPos;
     Quaternion InitialRot;

     static float tolerance = 2f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        InitialPos = transform.position;
        InitialRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            rigidbody.AddForce(transform.forward * force);

        if (Input.GetKey(KeyCode.S))
            rigidbody.AddForce(-transform.forward * force);

        if (Input.GetKey(KeyCode.A))
            transform.Rotate(0, -0.5f, 0);

        if (Input.GetKey(KeyCode.D))
            transform.Rotate(0, 0.5f, 0);


        if (transform.position.y < -8)
        {
            transform.position = InitialPos;
            transform.rotation = InitialRot;

            rigidbody.velocity = Vector3.zero;
        }

        print(transform.localEulerAngles);

        if (Mathf.Abs(transform.localRotation.eulerAngles.z - 90) < tolerance)
        {
            var zrot = transform.localRotation.eulerAngles.z;
            zrot /= zrot;
            rigidbody.AddRelativeTorque(new Vector3(0, 0, zrot));
            print("zrot: " + zrot);
        }

    }
}
