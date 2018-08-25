using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapons : MonoBehaviour
{


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



    public bool CanShoot
    {
        get
        {
            return PrevFireTime + FireFrequency < Time.time ? true : false;
        }

    }

    void Start()
    {
      
    }

    void Update()
    {
        KeyUpdate();
        PositionUpdate();
    }


    void Fire()
    {
        Debug.Log("开火");
    }
    
    void KeyUpdate()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            if(CanShoot)
            {
                Fire();
                PlayAllParticles();
                PrevFireTime = Time.time;
            }
        }
    }

    void PositionUpdate()
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
    }

#endif
}
