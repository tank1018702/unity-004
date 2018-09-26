using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    [SerializeField]
    Transform target;//test
    [SerializeField]
    float rotateSpeed=3f;

    [SerializeField]
    float Acceleration = 1000;


    protected override void Start()
    {
        base.Start();
        //test-------------------
        //target = GameObject.Find("testEnemy").transform;
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //Debug.DrawRay(transform.position, direction, Color.yellow);
        //transform.rotation = Quaternion.LookRotation(transform.forward, direction);
        transform.forward = direction;
    }

    protected override Vector3 MoveDirection(Vector3 direction)
    {
        if(target)
        {
            Vector3 tarDirection = (target.transform.position - transform.position).normalized;
            return Vector3.MoveTowards(transform.forward, tarDirection, rotateSpeed * Time.fixedDeltaTime);
        }
        return direction;
    }

    public virtual void Init(Vector3 _position, Vector3 _direction, float _speed, float _lifetime, int _damage, LayerMask _shooterLayer,Transform target)
    {
        transform.position = _position;
        direction = _direction;
        transform.forward = direction;
        speed = _speed;
        lifeTime = _lifetime;
        damage = _damage;
        shooterLayer = _shooterLayer;
        this.target = target;
    }

    protected override float SpeedUpdate(float speed)
    {
        return speed + Acceleration * Time.fixedDeltaTime;
    }

   
}
