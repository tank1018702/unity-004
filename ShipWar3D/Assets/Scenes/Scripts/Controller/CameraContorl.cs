using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContorl : MonoBehaviour
{
    public GameObject Target;

    public float smoothing = 3;





    void Start ()
    {
         
	}



    private void LateUpdate()
    {
        


    }
    private void FixedUpdate()
    {
        Vector3 tracePos = new Vector3(Target.transform.position.x, 100, Target.transform.position.z-60);
        transform.position = Vector3.Lerp(transform.position, tracePos, smoothing * Time.deltaTime);
    }
    void Update ()
    {

       
        

    }

    
}
