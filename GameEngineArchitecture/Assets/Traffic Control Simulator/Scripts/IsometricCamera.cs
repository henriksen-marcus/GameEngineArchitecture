using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCamera : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float speed = 0.1f;
    [SerializeField] float zoomSpeed = 1f;
    [SerializeField] float minCameraSize = 2;
    [SerializeField] float maxCameraSize = 150;
    [SerializeField] GameObject target;
    
    [Header("Input")]
    [SerializeField] KeyCode moveForwardKey = KeyCode.UpArrow;
    [SerializeField] KeyCode moveBackwardKey = KeyCode.DownArrow;
    [SerializeField] KeyCode moveLeftKey = KeyCode.LeftArrow;
    [SerializeField] KeyCode moveRightKey = KeyCode.RightArrow;
    
    private Vector3 _forward;
    private Camera _camera;
    
    void Start()
    {
        _forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        _camera = GetComponent<Camera>();
    }


    void Update()
    {
        var t = transform;
        if (Input.GetKey(moveForwardKey)) t.position += _forward * speed;
        if (Input.GetKey(moveBackwardKey)) t.position += -_forward * speed;
        if (Input.GetKey(moveLeftKey)) t.position += -t.right * speed;
        if (Input.GetKey(moveRightKey)) t.position += t.right * speed;

        float zoomDelta = _camera.orthographicSize - Input.mouseScrollDelta.y * zoomSpeed;
        _camera.orthographicSize = Mathf.Clamp(zoomDelta, minCameraSize, maxCameraSize);

        //transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        //Physics.Raycast(transform.position, transform.forward, out var hit, 1000f);
        
        //print(hit.distance);
    }
}
