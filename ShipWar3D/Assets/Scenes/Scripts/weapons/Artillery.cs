using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery : Emitter
{
    [SerializeField]
    private Transform model;
    public Transform Model
    {
        get
        {
            return model ? model : transform.Find("Model").transform;
        }
    }

    [SerializeField]
    private AnimationCurve LerpCurve = AnimationCurve.EaseInOut(0f, -0.4f, 0.4f, 0f);

    protected override void Update()
    {
        AnimUpdate();

        
    }

    void AnimUpdate()
    {
        
        float t = Time.time - PrevFireTime;
        model.localPosition = Vector3.forward * LerpCurve.Evaluate(t);
    }

    protected override void Shoot()
    {
        
        Instantiate(projectile).GetComponent<Projectile>().Init(muzzle.position, muzzle.forward, speed, lifeTime, damage, gameObject.layer);
    }

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        model = transform.Find("Model").transform;
    }
#endif
}
