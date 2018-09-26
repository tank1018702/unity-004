using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1000)]
public class AIController : MonoBehaviour
{
    Move movement;

    Emitter[] weapons;

    TurretsContorl[] Turrets;

    //寻敌范围
    public float radiusForSearchTarget;

    //目标距离的余量
    public float stopDistence;

    //目标夹角的余量
    public float stopDegrees;

    //目标
    GameObject target = null;


    void Start ()
    {
        movement = transform.GetComponent<Move>();

        //weapons = transform.GetComponentsInChildren<Emitter>(true);

        Turrets = transform.GetComponentsInChildren<TurretsContorl>(true);       
    }
		
	void Update ()
    {
        SearchEnemy();
	}

    void SearchEnemy()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, radiusForSearchTarget, 1 << LayerMask.NameToLayer("Player"));

        //玩家只有一个,先不考虑多对多的战斗
        if(col.Length>0)
        target = col[0].gameObject ?? null;

        Vector3 targetPos;

        if (target != null)
        {

            targetPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        }
        else
        {
            targetPos = Vector3.zero;
        }
        AIControl(target != null, targetPos);
    }


    void MoveToTarget(Vector3 targetPos_fixed)
    {
        
        float dis = Vector3.Distance(targetPos_fixed, transform.position);

        if(dis>stopDistence)
        {
            movement.speedUp = true;
        }
        else
        {
            movement.speedUp = false;
        }
    }

    void AimAtTarget(Vector3 targetPos_fixed)
    {
        for(int i=0;i<Turrets.Length;i++)
        {
            Turrets[i].targetPos = targetPos_fixed;
            
            Turrets[i].Fire();
        }
    }

 

    void RotateToTarget(Vector3 targetPos_fixed)
    {  
        Vector3 tarDir =(targetPos_fixed - transform.position).normalized;

        bool tarIsRight = Vector3.Cross(transform.forward, tarDir).y > 0;

        float angle = Vector3.Angle(transform.forward, tarDir);
        if(angle>stopDegrees)
        {     
            if (tarIsRight)
            {
                movement.turnRight = true;
                movement.turnLeft = false;
            }
            else
            {
                movement.turnRight = false;
                movement.turnLeft = true;
            }
        }
        else
        {
            movement.turnRight = false;
            movement.turnRight = false;
        }
    }

    void AIControl(bool hasTarget,Vector3 targetPos_fixed)
    {
        if(!hasTarget)
        {
            movement.speedUp = false;
            movement.turnRight = false;
            movement.turnLeft = false;
            for(int i=0;i<Turrets.Length;i++)
            {
                Turrets[i].targetPos = targetPos_fixed;
            }
            return;
        }
        MoveToTarget(targetPos_fixed);
        RotateToTarget(targetPos_fixed);
        AimAtTarget(targetPos_fixed);
        
    }
    //寻敌范围可视化
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusForSearchTarget);
    }
}
