using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class PlayerController : DartTagPlayer
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0))
        {
            ShootDart();
        }
    }
}
