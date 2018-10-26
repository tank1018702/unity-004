using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : Projectile
{

    public float velocityDecay;

    Transform sea;

    protected override void Start()
    {
        base.Start();
        sea = GameObject.Find("Sea").transform;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();     
    }

    protected override void MoveLogicUpdate()
    {
        speed -= speed < 0 ? 0 : velocityDecay * Time.fixedDeltaTime;
        distance = speed * Time.fixedDeltaTime;
        transform.position += direction * distance;
    }

    protected override void PlayParticleAtPoint(ParticleSystem pc, Vector3 point, Vector3 direction)
    {
        //Debug.Log(new Vector3(point.x, 0, point.z)+":"+point);
        //有时候击中坐标为零,很诡异
        if(point!=Vector3.zero)
        {
            pc.transform.position = new Vector3(point.x, 0, point.z);
        }
        else
        {
            pc.transform.position = transform.position;
        }
        pc.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //设置水花击中海面的trigger
        pc.transform.GetChild(0).GetComponent<ParticleSystem>().trigger.SetCollider(0, sea);   
        pc.Play();
    }
}
