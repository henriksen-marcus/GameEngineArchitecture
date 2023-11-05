using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class CylSurface : BallCollisionHandler
{
    public override int scoreAmount { get; set; } = 100;
    
    protected override void HandleTriggerEnter()
    {
        var rb = _other.GetComponent<Rigidbody>();
        rb.AddExplosionForce(300f, transform.position, 1f);
    }
}
