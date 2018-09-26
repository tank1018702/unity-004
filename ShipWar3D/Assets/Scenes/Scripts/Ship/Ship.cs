using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public int hp=100;


	
	void Start ()
    {
		
	}
	
	
	void Update ()
    {
        Debug.Log(hp);
	}

    public void BeHit(int damage)
    {
        hp = hp < 0 ? 0 : hp - damage;
        if(hp==0)
        {
            Die();
        }
        

    }

    void Die()
    {
        Debug.Log("Die");
    }
}
