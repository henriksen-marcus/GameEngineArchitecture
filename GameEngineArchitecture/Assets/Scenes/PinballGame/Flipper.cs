using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Flipper : MonoBehaviour
{
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float timer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            timer = 0;
        }

        timer += Time.deltaTime;
        
        // Rot logic
        var localRot = transform.localRotation;
        var angle = animationCurve.Evaluate(timer);
        localRot.z = angle;
        transform.localRotation = localRot;
    }
}
