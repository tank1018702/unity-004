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

    [SerializeField]
    float lead = 0.5f;
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

    protected override void MoveLogicUpdate()
    {
        if (target)
        {
            Vector3 offsetPos = new Vector3(target.transform.position.x, target.transform.position.y + 1f, target.transform.position.z);
            Vector3 tarDirection = (offsetPos - transform.position).normalized;
            //提前量
            //Vector3 leadPos = target.transform.position + target.transform.GetComponent<Move>().CurVeloticy.normalized;
            direction = Vector3.MoveTowards(transform.forward, tarDirection, rotateSpeed * Time.fixedDeltaTime).normalized;
        }

        speed += Acceleration * Time.fixedDeltaTime;
        distance = speed * Time.fixedDeltaTime;
        transform.position += direction * distance;
    }


    public void Init(Vector3 _position, Vector3 _direction, float _speed, float _lifetime, int _damage, GameObject shooter,  Transform target)
    {
        transform.position = _position;
        direction = _direction;
        transform.forward = direction;
        speed = _speed;
        lifeTime = _lifetime;
        damage = _damage;
        this.shooter = shooter;
        this.target = target;
        //
        //hitable = hitable | player | enemy & (~1 << shooter.layer);
        hitable = hitable | player | enemy;
        //Debug.Log(Convert.ToString(hitable, 2));
        hitable = hitable & (~(1 << shooter.layer));
    }

   
}
