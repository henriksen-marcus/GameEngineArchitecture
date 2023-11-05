using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringSurface : BallCollisionHandler
{
    public override int scoreAmount { get; set; } = 200;

    protected override void HandleTriggerEnter()
    {
        var rb = _other.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 200f);
    }
}
