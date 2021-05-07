using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBController : EnemyBase
{
    protected override void Update()
    {
        Wander();

        if (playerDetected)
        {
            Ray ray = new Ray(transform.position, Vector2.right);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);
            //I don't think I want the actual ship to do anything except shoot, for now.
            //Will look into positioning into broadside, but that'll take too long ATM.
        }
    }
}
