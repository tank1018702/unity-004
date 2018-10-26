using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : Emitter
{
    

    public int TotalMissileNumber;

    int curMissileNumber;

    public float SingleReLoadingTime;

    float prveLodingTime = float.MinValue;

    bool ReLoding
    {
        get
        {
            return SingleReLoadingTime + prveLodingTime < Time.time;
        }
    }

    protected override void Start()
    {
        base.Start();
        curMissileNumber = TotalMissileNumber;
    }

    public override void Fire()
    {
        if (ReadyToShoot&&curMissileNumber>0)
        {
            Shoot();
            PlayAllParticles();
            PrevFireTime = Time.time;
            curMissileNumber = Mathf.Clamp(curMissileNumber - 1, 0, TotalMissileNumber);
        }
    }


    protected override void Update()
    {
        LoadingTimeUpdate();
        
    }

    protected override void Shoot()
    {     
        //发射角太低了容易射到水里..
        //更正,是目标坐标位置默认在船底,在Y轴给一个偏移量就行
        Vector3 randomForward = new Vector3(muzzle.forward.x, Random.Range(muzzle.forward.y +0.5f, muzzle.forward.y + 0.8f), muzzle.forward.z);
        Instantiate(projectile).GetComponent<Missile>().Init(muzzle.position, randomForward, speed, lifeTime, damage,this.gameObject, target);
    }

    void LoadingTimeUpdate()
    {
        if (ReLoding)
        {
            curMissileNumber = Mathf.Clamp(curMissileNumber + 1, 0, TotalMissileNumber);
            prveLodingTime = Time.time;
        }

    }
}
