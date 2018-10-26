using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacherController : ShipController
{


    public Camera cam;



    protected override void ControlUpdate()
    {
        movement.speedUp = Input.GetKey(KeyCode.W);
        movement.turnLeft = Input.GetKey(KeyCode.A);
        movement.turnRight = Input.GetKey(KeyCode.D);


        Vector3 v = GetMousePos();
        for (int i = 0; i < Turrets.Length; i++)
        {
            Turrets[i].targetPos = v;
            if (Input.GetMouseButton(0))
            {
                Turrets[i].Fire();
            }
        }

    }

    Vector3 GetMousePos()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

   
}
