using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacherController : MonoBehaviour
{
    Move movement;

    [SerializeField]
    Emitter [] weapons;

    [SerializeField]
    TurretsContorl [] Turrets;

    public Camera cam;

	
	void Start ()
    {
        movement = transform.GetComponent<Move>();

        weapons = transform.GetComponentsInChildren<Emitter>(true);

        Turrets = transform.GetComponentsInChildren<TurretsContorl>(true);
    }
	

    void InputUpdate()
    {
        movement.speedUp = Input.GetKey(KeyCode.W);
        movement.turnLeft = Input.GetKey(KeyCode.A);
        movement.turnRight = Input.GetKey(KeyCode.D);

     
        Vector3 v = GetMousePos();
        for(int i=0;i<Turrets.Length;i++)
        {
            Turrets[i].targetPos = v;
            if(Input.GetMouseButton(0))
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
	
	void Update ()
    {
        InputUpdate();		
	}
}
