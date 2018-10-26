using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Debug.Log(GetComponent<ParticleSystem>().trigger.GetCollider(0).ToString());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
