using System.Collections;
using System.Collections.Generic;
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

    RaycastHit[] hit;

    public ParticleSystem hitTarget;
    public ParticleSystem hitSeaSurface;
    public ParticleSystem disappear;

    //先放着
    protected LayerMask hitAbleLayer;
    protected LayerMask groundLayer;
    protected LayerMask shooterLayer;
    protected LayerMask tempLayer;

    protected virtual void Start()
    {

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
        speed = SpeedUpdate(speed);
        distance = speed * Time.fixedDeltaTime;
        if (Physics.SphereCastNonAlloc(transform.position, 0.15f, direction, hit, distance, hitAbleLayer) > 0)
        {
            tempLayer = hit[0].collider.gameObject.layer;
            distance = hit[0].distance;
            if (((1 << tempLayer) & shooterLayer) == 0)
            {
                //PlayParticleAtPoint(hitTarget, hit[0].point, hit[0].normal);
                hit[0].collider.GetComponent<Ship>().BeHit(damage);
            }
            else if (((1 << tempLayer) & groundLayer) != 0)
            {


            }
            Destroy(gameObject);
        }
        direction = MoveDirection(direction);
        transform.position += direction * distance;
    }

    protected virtual float SpeedUpdate(float speed)
    {
        return speed;
    }
    protected virtual Vector3 MoveDirection(Vector3 direction)
    {
        return direction;
    }


    //初始发射物的属性
    public virtual void Init(Vector3 _position, Vector3 _direction, float _speed, float _lifetime, int _damage, LayerMask _shooterLayer)
    {
        transform.position = _position;
        direction = _direction;
        transform.forward = direction;
        speed = _speed;
        lifeTime = _lifetime;
        damage = _damage;
        shooterLayer = _shooterLayer;
    }
    void PlayParticleAtPoint(ParticleSystem pc, Vector3 point, Vector3 direction)
    {
        pc.transform.position = point;
        pc.transform.rotation = Quaternion.LookRotation(direction);
        pc.Play();
    }
}
