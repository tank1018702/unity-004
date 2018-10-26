using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoTube : Emitter
{

    public float SpreadAngle;

    public int TotalNumber;

    Vector3[] Directions;

    protected override void Start()
    {
        base.Start();

        Directions = new Vector3[TotalNumber];
    }


    protected override void Update()
    {

    }

    protected override void Shoot()
    { 
        RaycastHit hit;
        GetDirections(SpreadAngle, TotalNumber);
        int layerMask =1<< LayerMask.NameToLayer("Water");
        
        if (Physics.Raycast(muzzle.position, -Vector3.up, out hit, 10f, ~layerMask))
        {    
            for(int i=0;i<Directions.Length;i++)
            {
                Instantiate(projectile).GetComponent<Projectile>().Init(new Vector3(hit.point.x,hit.point.y-0.55f,hit.point.z), Directions[i], speed, lifeTime, damage,this.gameObject);
            }          
        }
        else
        {
            Debug.Log("layerError");
        }
        //Debug.DrawRay(muzzle.position, -Vector3.up * 10, Color.red, 10f);
    }

    void GetDirections(float spreadAngle, int num)
    {
        Vector3 baseDir = Quaternion.AngleAxis(-spreadAngle / 2, Vector3.up) * new Vector3(muzzle.forward.x, 0, muzzle.forward.z);
        float eachAngle = spreadAngle / num;
        for (int i = 0; i < TotalNumber; i++)
        {
            Directions[i] = Quaternion.AngleAxis(i * eachAngle, Vector3.up) * baseDir;
        }
    }
}
