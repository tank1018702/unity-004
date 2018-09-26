using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapons : MonoBehaviour
{
    [SerializeField]
    bool hasAnim = true;

    [SerializeField]
    private Transform model;
    public Transform Model
    {
        get
        {
            return model ? model : transform;
        }
    }
    [SerializeField]
    private float FireFrequency = 0.4f;
    [SerializeField]
    private float PrevFireTime = float.MinValue;

    [SerializeField]
    private AnimationCurve LerpCurve =AnimationCurve.EaseInOut(0f, -0.4f, 0.4f, 0f);
    [SerializeField]
    private ParticleSystem[] Particles=new ParticleSystem[0];

    public Transform muzzle;

    bool controllerIsPlayer = false;

    [SerializeField]
    TurretsContorl turrets;

    [SerializeField]
    GameObject bullet;


    public float  speed;

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



    void Start()
    {
        Transform root = GetRoot(transform);
        
        if (root.tag== "Player")
        {      
            controllerIsPlayer = true;   
        }
        if(!turrets)
        {
            turrets = transform.GetComponentInParent<TurretsContorl>();
        }
        if(!muzzle)
        {
            muzzle = transform.Find("Muzzle").transform;
        }
       

    }

    Transform GetRoot(Transform t)
    {
        if(t.parent==null)
        {
            return t;
        }
        else
        {
            return GetRoot(t.parent);
        }
    }

    void Update()
    {
        if (hasAnim)
            AnimUpdate();
    }


    void Shoot()
    {
        Instantiate(bullet).GetComponent<Projectile>().Init(muzzle.position, muzzle.forward, speed, lifeTime, damage, gameObject.layer);
        
    }

    public void Fire()
    {

        if (ReadyToShoot)
        {
            Shoot();
            PlayAllParticles();
            PrevFireTime = Time.time;
        }

    }

    void AnimUpdate()
    {
        float t = Time.time - PrevFireTime;
        model.localPosition = Vector3.forward * LerpCurve.Evaluate(t);
    }
    
    void PlayAllParticles()
    {
        for(int i=0;i<Particles.Length;i++)
        {
            Particles[i].Play();
        }
    }

  

#if UNITY_EDITOR

    private void Reset()
    {
        model = transform.GetChild(0).transform;

        ParticleSystem[] p = GetComponentsInChildren<ParticleSystem>(true);
        Particles = p;

        muzzle = transform.Find("Muzzle").transform;
        turrets = transform.GetComponentInParent<TurretsContorl>();
    }

#endif

}
