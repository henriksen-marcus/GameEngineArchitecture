using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Flipper : MonoBehaviour
{
    [SerializeField] private KeyCode Button;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float _timer = 1f;
    [SerializeField] private bool _isLeftFlipper;
    private Quaternion _initialRotation;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = gameObject.GetComponentInChildren<Rigidbody>();
        _initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(Button))
        {
            _timer = 0;
            print("Reset");
        }
        
        _timer += Time.deltaTime;
    }
    
    private void FixedUpdate() {
        int direction = _isLeftFlipper ? -1 : 1;
        _rb.MoveRotation(_initialRotation * Quaternion.Euler(0,0,animationCurve.Evaluate(_timer) * 90f * direction));
    }
}
