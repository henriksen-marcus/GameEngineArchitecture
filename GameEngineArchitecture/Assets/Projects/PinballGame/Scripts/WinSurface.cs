using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinSurface : BallCollisionHandler
{
    public override int scoreAmount { get; set; } = 500;

    protected override void HandleTriggerEnter()
    {
    }
}