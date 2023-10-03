using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCamera : MonoBehaviour
{
    [SerializeField] float speed = 0.1f;
    [SerializeField] float zoomSpeed = 1f;
    [SerializeField] float minCameraSize = 2;
    [SerializeField] float maxCameraSize = 150;
    
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
        if (Input.GetKey(KeyCode.W)) t.position += _forward * speed;
        if (Input.GetKey(KeyCode.S)) t.position += -_forward * speed;
        if (Input.GetKey(KeyCode.A)) t.position += -t.right * speed;
        if (Input.GetKey(KeyCode.D)) t.position += t.right * speed;

        float zoomDelta = _camera.orthographicSize - Input.mouseScrollDelta.y * zoomSpeed;
        _camera.orthographicSize = Mathf.Clamp(zoomDelta, minCameraSize, maxCameraSize);
    }
}
