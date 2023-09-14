using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Flipper : MonoBehaviour
{
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float _timer = 1f;
    private Quaternion _initialRotation;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = gameObject.GetComponentInChildren<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _timer = 0;
        }

        _timer += Time.deltaTime;
        
        // Rot logic
        //var localRot = transform.localRotation;
        //var angle = animationCurve.Evaluate(_timer);
        //localRot.y = angle;
        //transform.localRotation = localRot;

        _rb.MoveRotation(_initialRotation * Quaternion.Euler(0, 0, animationCurve.Evaluate(_timer) * 90f));
        //_rb.MoveRotation(_startRot * Quaternion.Euler(0, 0, animationCurve.Evaluate(_timer) * 90f));
    }
}
