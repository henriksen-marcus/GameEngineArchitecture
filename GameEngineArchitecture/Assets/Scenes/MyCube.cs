using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCube : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    private Camera _cam;

    void Awake()
    {
        _cam = GameObject.FindObjectOfType<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxis("Horizontal") * Time.deltaTime * _speed;
        var v = Input.GetAxis("Vertical") * Time.deltaTime * _speed;
        
        transform.position += new Vector3(h, 0,  v);
    }

    void FixedUpdate()
    {
    }
}
