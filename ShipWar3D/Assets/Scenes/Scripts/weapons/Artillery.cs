using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery : Emitter
{
    [SerializeField]
    private Transform model;

    float spreadRadius;

    Vector3 TargetDropPos=Vector3.zero;

    /// <summary>
    /// 最大仰角
    /// </summary>
    public float MaxElevation=10f;
    /// <summary>
    /// 最大俯角
    /// </summary>
    public float MaxDepression=-5f;

    float AngleOfPitch;

    float G = 10f;
    [SerializeField]
    float d_offset=0;

    public Transform Model
    {
        get
        {
            return model ? model : transform.Find("Model").transform;
        }
    }


    [SerializeField]
    private AnimationCurve LerpCurve = AnimationCurve.EaseInOut(0f, -0.4f, 0.4f, 0f);

    TurretsContorl t;

    public Mesh mesh;
    Vector3 drawPos = Vector3.zero;

    protected override void Start()
    {
        base.Start();
        t = GetComponentInParent<TurretsContorl>();
    }

    protected override void Update()
    {
        AnimUpdate();
        float velocity = target.GetComponent<Rigidbody>().velocity.magnitude;
      
        drawPos= CalculateLeadPos(target.position, velocity, target.forward, 1f, target.position, float.MaxValue);
        t.targetPos = drawPos;
       //GetFireAngle(target.position);
       RotateUpdate();

    }

    public void GetTargetDrop()
    {
        Vector3 targetPos = transform.GetComponentInParent<TurretsContorl>().targetPos;
        TargetDropPos = new Vector3(targetPos.x, 0, targetPos.z);
    }

    


    void AnimUpdate()
    {   
        float t = Time.time - PrevFireTime;
        model.localPosition = Vector3.forward * LerpCurve.Evaluate(t);
    }

    Vector3 Vector_Y2Zero(Vector3 v)
    {
        return new Vector3(v.x, 0, v.z);
    }

    void SimulationDropPos_visualization(Vector3 direction)
    {
        float time = 0;

        Ray ray=new Ray();
        RaycastHit hit;
        Vector3 move=Vector3.zero;

        Vector3 curPos = muzzle.position;

        while (!Physics.Raycast(ray,out hit,move.magnitude,1<<LayerMask.NameToLayer("Sea"))&&curPos.y>0)
        {
            Vector3 gravDir = Vector3.down * G * time;

            move = (direction * speed + gravDir)*Time.fixedDeltaTime;

            ray = new Ray(curPos, move);
            Debug.DrawRay(curPos, move, Color.red);
            curPos += move;
            time += Time.fixedDeltaTime;
        }
        if(hit.transform)
        {
            curPos = hit.point;
        }
       
        Debug.DrawRay(curPos, Vector3.up * 10, Color.black);
    }


    //accuracy(精度)给得越高,递归计算的次数也会越多
    public Vector3 CalculateLeadPos(Vector3 tarPos,float tarSpeed,Vector3 tarTowards,float accuracy,Vector3 sim_Point,float diff)
    {
        if (sim_Point==Vector3.zero)
        {
            return Vector3.zero;
        }
        Vector3 tarDir = (Vector_Y2Zero(sim_Point) - Vector_Y2Zero(transform.parent.position)).normalized;
        Quaternion tarRotation = Quaternion.FromToRotation(tarDir, Vector3.forward);
        Vector3 LocalHitPos = tarRotation * (sim_Point- muzzle.position);

        float V = speed;
        float X = LocalHitPos.z;
        float Y = -LocalHitPos.y+d_offset;
        Vector2 TT = SimulationProjectile(X, Y, V, G);
        
        if(TT==Vector2.zero)
        {
            return Vector3.zero;
        }
        Vector3 newSim_point = Sim_DropPos(tarSpeed, tarPos, tarTowards, TT.y);
        float curDiff = (newSim_point - sim_Point).magnitude;
        if(curDiff>diff)
        {
            Debug.Log("Error:Out Of Range Or Other");
            return Vector3.zero;
        } 
        if (curDiff<accuracy)
        {
            Debug.DrawRay(newSim_point, Vector3.up * 10, Color.yellow);
            AngleOfPitch = TT.x * Mathf.Rad2Deg;
            return  newSim_point;
        }
        return CalculateLeadPos(tarPos, tarSpeed, tarTowards, accuracy, newSim_point, curDiff);
    }

    Vector2 SimulationProjectile(float X, float Y, float V, float G)
    {
        if (G == 0)
        {
            float THETA = Mathf.Atan(Y / X);
            float T = (Y / Mathf.Sin(THETA)) / V;
            return (new Vector2(THETA, T));
        }
        else
        {
            float DELTA = Mathf.Pow(V, 4) - G * (G * X * X - 2 * Y * V * V);
            if (DELTA < 0)
            {
                return Vector2.zero;
            }
            float rad1 = Mathf.Atan(((V * V) + Mathf.Sqrt(DELTA)) / (G * X));
            float rad2 = Mathf.Atan(((V * V) - Mathf.Sqrt(DELTA)) / (G * X));
         
            float rad = Mathf.Min(rad1, rad2);
            float T = X / (V * Mathf.Cos(rad));
            return new Vector2(rad, T);
        }
    }

    Vector3 Sim_DropPos(float speed, Vector3 curPos, Vector3 tarTowards, float time)
    {
        Vector3 sim_pos = Vector_Y2Zero(curPos) + Vector_Y2Zero(tarTowards) * (speed * time);
        return sim_pos;
    }
    //弃用
    public float GetFireAngle(Vector3 tarPos)
    {

        if (tarPos == Vector3.zero) { return 0f; }


        Vector3 Dir = (Vector_Y2Zero(tarPos) - Vector_Y2Zero(transform.parent.position)).normalized;

        float d_aix2muzzle = (Vector_Y2Zero(transform.parent.position) - Vector_Y2Zero(muzzle.position)).magnitude;
        float d_tar2aix = (Vector_Y2Zero(transform.parent.position) - Vector_Y2Zero(tarPos)).magnitude;

        float d = d_tar2aix - d_aix2muzzle + d_offset;
        float h = muzzle.transform.position.y;

        float X = d;
        float Y = h;
        float V = speed;
        //Debug.Log(" X:" + X + " Y:" + Y + " V:" +V+ " G:" + G);
        float angle = SimulationProjectile(X,Y,V,G).x;



        //float a = Mathf.Sqrt((speed * speed * speed * speed - G * (G * d * d - 2 * h * speed * speed)));
        //float tanAngle_1 = ((speed * speed + a) / (G * d));
        //float tanAngle_2 = ((speed * speed - a) / (G * d));
        //float a1 = Mathf.Atan(tanAngle_1) * Mathf.Rad2Deg;
        //float a2 = Mathf.Atan(tanAngle_2) * Mathf.Rad2Deg;



        //float angle = Mathf.Min(a1, a2);

        //Debug.Log("old"+a1 + ":" + a2);

        AngleOfPitch = angle*Mathf.Rad2Deg;

        return angle;
    }

    void RotateUpdate()
    {
        //限制俯仰角
        float angle = Mathf.Clamp(AngleOfPitch,MaxDepression,MaxElevation) ;
        
        //Debug.Log(angle);

        transform.localRotation = Quaternion.Euler(new Vector3(-angle, 0, 0));
        SimulationDropPos_visualization(transform.forward);
        //transform.RotateAround(transform.position, transform.right, angle);

    }

    protected override void Shoot()
    {  
        Instantiate(projectile).GetComponent<Projectile>().Init(muzzle.position, transform.forward, speed, lifeTime, damage, this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawMesh(mesh, drawPos, target.transform.rotation);

    }

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        model = transform.Find("Model").transform;
    }
#endif
}
