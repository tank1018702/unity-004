using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    [SerializeField]
    protected float FireFrequency = 0.4f;

    protected float PrevFireTime = float.MinValue;


    [SerializeField]
    protected ParticleSystem[] Particles = new ParticleSystem[0];

    [SerializeField]
    protected Transform muzzle;

    protected bool controllerIsPlayer = false;

    [SerializeField]
    protected TurretsContorl turrets;

    [SerializeField]
    protected GameObject projectile;


    public float speed;

    public float lifeTime;

    public int damage;

    public bool ReadyToShoot
    {
        get
        {
            if (controllerIsPlayer)
            {
                return PrevFireTime + FireFrequency < Time.time;
            }
            else
            {
                return (PrevFireTime + FireFrequency < Time.time) && turrets.inShootArea;
            }
        }

    }
    protected virtual void Start()
    {
        Transform root = GetRoot(transform);

        if (root.tag == "Player")
        {
            controllerIsPlayer = true;
        }
        if (!turrets)
        {
            turrets = transform.GetComponentInParent<TurretsContorl>();
        }
        if (!muzzle)
        {
            muzzle = transform.Find("Muzzle").transform;
        }

    }
    protected virtual void Update()
    {

    }

    Transform GetRoot(Transform t)
    {
        if (t.parent == null)
        {
            return t;
        }
        else
        {
            return GetRoot(t.parent);
        }
    }
    protected virtual void Shoot()
    {
       

    }

    public virtual void Fire()
    {
        if (ReadyToShoot)
        { 
            Shoot();
            PlayAllParticles();
            PrevFireTime = Time.time;
        }
    }
  protected  void PlayAllParticles()
    {
        for (int i = 0; i < Particles.Length; i++)
        {
            Particles[i].Play();
        }
    }
#if UNITY_EDITOR

    protected virtual void Reset()
    {
        ParticleSystem[] p = GetComponentsInChildren<ParticleSystem>(true);
        Particles = p;

        muzzle = transform.Find("Muzzle").transform;
        turrets = transform.GetComponentInParent<TurretsContorl>();
    }
#endif
}
