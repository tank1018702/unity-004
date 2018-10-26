using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    bool IsDead = false;

    
   protected Move movement;

    [SerializeField]
    protected  TurretsContorl[] Turrets;

   protected virtual void Start()
    {
        movement = transform.GetComponent<Move>();
        Turrets = transform.GetComponentsInChildren<TurretsContorl>(true);
    }

    protected virtual void Update()
    {
       
    }

    protected virtual void FixedUpdate()
    {
        if (IsDead) { return; }
        ControlUpdate();
    }
    public  void StateInit()
    {
        IsDead = true;
        StartCoroutine(TurnOff());

    }

    IEnumerator TurnOff()
    {
        
        movement.speedUp = false;
        movement.turnRight = false;
        movement.turnLeft = false;
        yield return new WaitForSeconds(3f);
        movement.Dead = true;
    }

    protected virtual void ControlUpdate()
    {

    }
}
