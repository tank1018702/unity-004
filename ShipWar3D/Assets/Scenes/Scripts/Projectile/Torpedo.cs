using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : Projectile
{

    public float velocityDecay;


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
    }

    protected override float SpeedUpdate(float speed)
    {
        speed -= velocityDecay * Time.fixedDeltaTime;

        return speed<0?0:speed;
        
    }
}
