using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : BallCollisionHandler
{
    public override int scoreAmount { get; set; } = 0;

    protected override void HandleTriggerEnter()
    {
        var rb = _other.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 520f);
    }
}
