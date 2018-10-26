using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    
    float g = 10f;

    float time = 0;

    Vector3 initDiretcion;

    protected override void MoveLogicUpdate()
    {

      
        //实际移动方向
        Vector3 move;
        
        Vector3 gravDir = Vector3.down * g *time;

        
        move = initDiretcion * speed  + gravDir;

        direction = move.normalized;

        distance = move.magnitude*Time.fixedDeltaTime;

        
        transform.position += move * Time.fixedDeltaTime;

        time += Time.fixedDeltaTime;
    }
    public override void Init(Vector3 _position, Vector3 _direction, float _speed, float _lifetime, int _damage, GameObject shooter)
    {
        base.Init(_position, _direction, _speed, _lifetime, _damage, shooter);

        initDiretcion = _direction;
    }
   

}
