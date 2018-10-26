using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //阻力
    float Resistance
    {
        get
        {
            return ResistanceCurve.Evaluate(CurVeloticy.magnitude);
        }
    }


    public float enginePower = 100;

    float speedAmount = 0f;

    float acceleration
    {
        get
        {
            return Mathf.Max(0.1f, enginePower - (ShipMass + Resistance));
        }
    }


    float RotateSpeed = 720;
 

    //EngineState engineState;

    Rigidbody rig;

    public AnimationCurve ResistanceCurve = new AnimationCurve();

    //base
    public float ShipMass = 80f;

    public float ShipLength = 20f;




    //move
    Vector3 PrevPosition;

    public Vector3 CurVeloticy;

    Vector3 TargetVeloticy;

    Vector3 ActualVeloticy;



    Vector3 ForwardAmount;

    float TurnAmount;

    float ActualTurnAmount;

     public float maxRotateVelotocy=2;
 



    //control
    public bool speedUp = false;

    public bool turnLeft = false;

    public bool turnRight = false;

    public bool Dead = false;


    void Start()
    {

        rig = GetComponent<Rigidbody>();
        
        PrevPosition = transform.position;
        ActualVeloticy = Vector3.zero;
        ForwardAmount = transform.forward;
        transform.position = new Vector3(transform.position.x, 0, transform.position.y);
    }

    private void FixedUpdate()
    {
        InputProcess();
        InterpolationVeloticy();
        MovementUpdate();
    }

   

    void InterpolationVeloticy()
    {
        ActualVeloticy = Vector3.Lerp(ActualVeloticy, TargetVeloticy, 0.005f);

        ActualTurnAmount = Mathf.Lerp(ActualTurnAmount, TurnAmount, 0.1f);
    }



    void MovementUpdate()
    {
        if(Dead)
        {
            rig.velocity = Vector3.zero;
            return;
        }
        rig.velocity = Quaternion.AngleAxis(ActualTurnAmount, transform.up) * ActualVeloticy;


        if (rig.velocity.normalized != Vector3.zero)
        {
            transform.forward = rig.velocity.normalized;
        }

        CurVeloticy = transform.position - PrevPosition;
        PrevPosition = transform.position;
    }


    void InputProcess()
    {
        if (speedUp)
        {
            TargetVeloticy = ForwardAmount * (speedAmount + acceleration);
        }
        else
        {
            TargetVeloticy = ForwardAmount * (Mathf.Max(0, speedAmount - Resistance));
        }

        float rotateRate = Mathf.Clamp(CurVeloticy.magnitude / maxRotateVelotocy, 0f, 1f);
      
        float deg = RotateSpeed *rotateRate;
        
        if (turnLeft)
        {     
            TurnAmount -= deg * Time.deltaTime;
        }
        if (turnRight)
        {
            TurnAmount += deg * Time.deltaTime;
        }   
    }




    //float EngineUpdate()
    //{      
    //    float engine_rate = 0f;

    //    switch (engineState)
    //    {
    //        case EngineState.FirstGear:
    //            engine_rate = 0.2f;
    //            break;
    //        case EngineState.SecondGear:
    //            engine_rate = 0.4f;
    //            break;
    //        case EngineState.ThirdGear:
    //            engine_rate = 0.8f;
    //            break;
    //        case EngineState.FourthGear:
    //            engine_rate = 1.0f;
    //            break;
    //        case EngineState.Stop:
    //            engine_rate = 0f;
    //            break;
    //    }
    //    return engine_rate;
    //}   
}

//enum EngineState
//{
//    FourthGear=4,
//    ThirdGear=3,
//    SecondGear=2,
//    FirstGear=1,
//    Stop=0
//}