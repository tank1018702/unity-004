using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;



public class Projectile : MonoBehaviour
{
    [SerializeField]
    protected Vector3 direction;
    [SerializeField]
    protected float lifeTime;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float distance;
    [SerializeField]
    protected int damage;
    [SerializeField]
    float radius=0.15f;

    RaycastHit[] hit;

    public ParticleSystem hitTarget;
    public ParticleSystem hitWater;
    public ParticleSystem hitGround;
    public ParticleSystem disappear;

    protected GameObject shooter;

   
    [SerializeField]
    protected LayerMask hitable;
    protected LayerMask templayer;

    [SerializeField]
    protected LayerMask player;
    [SerializeField]
    protected LayerMask enemy;
    [SerializeField]
    protected LayerMask ground;
    [SerializeField]
    protected LayerMask water;

    


    protected virtual void Start()
    {
        hit = new RaycastHit[1];    
    }

    protected virtual void FixedUpdate()
    {
        MoveUpdate();
    }

    virtual protected void MoveUpdate()
    {
        lifeTime -= Time.fixedDeltaTime;
        if (lifeTime < 0)
        {
            if (disappear != null)
            {
                disappear.Play();
            }
            Destroy(gameObject);
            //Invoke("Destroy(gameObject)",disappear.time);
        }
        MoveLogicUpdate();
        
        //Debug.DrawRay(transform.position, direction * distance, Color.red, Time.deltaTime);
        HitUpdate();     
        
    }


    protected virtual void HitUpdate()
    {
        if (Physics.SphereCastNonAlloc(transform.position, radius, direction.normalized, hit, distance, hitable) > 0)
        {
            
            templayer = hit[0].collider.gameObject.layer;
            distance = hit[0].distance;  
            if (((1 << templayer) & (player | enemy)) != 0)
            {    
                if(hitTarget)
                PlayParticleAtPoint(Instantiate(hitTarget), hit[0].point, hit[0].normal);
                hit[0].collider.GetComponentInParent<Ship>().BeHit(damage);
            }
            else if (((1 << templayer) & ground) != 0)
            {

            }
            else if (((1 << templayer) & water) != 0)
            {          
                if (hitWater)
                    PlayParticleAtSurface(Instantiate(hitWater), hit[0].point);      
            }
            Destroy(gameObject);
        }
    }

    protected virtual void MoveLogicUpdate()
    {
        
    }



    //初始发射物的属性
    public virtual void Init(Vector3 _position, Vector3 _direction, float _speed, float _lifetime, int _damage, GameObject shooter)
    {
        transform.position = _position;
        direction = _direction;
        transform.forward = direction;
        speed = _speed;
        lifeTime = _lifetime;
        damage = _damage;
        this.shooter = shooter;
        //还是放这里
        //过滤掉自身的层级
        hitable = hitable | player | enemy;
        //Debug.Log(Convert.ToString(hitable, 2));
        hitable = hitable &(~(1 << shooter.layer));
        //Debug.Log(Convert.ToString(1 << shooter.layer, 2));
        //Debug.Log(Convert.ToString(hitable, 2));
        //Debug.Log(Convert.ToString(hitable, 2)+":"+Convert.ToString(1<<shooter.layer, 2)+":"+ Convert.ToString(hitable&~1<<shooter.layer, 2));

    }
    protected virtual void PlayParticleAtPoint(ParticleSystem pc, Vector3 point, Vector3 direction)
    {
        pc.transform.position = point;
        pc.transform.rotation = Quaternion.LookRotation(direction);
        pc.Play();
    }

    protected virtual void PlayParticleAtSurface(ParticleSystem pc,Vector3 point)
    {
        pc.transform.position = new Vector3(point.x, 0, point.z);
        pc.transform.rotation = Quaternion.identity;
        pc.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
