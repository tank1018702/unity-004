using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public int hp = 100;

    ShipController controller;

    public ParticleSystem explosion;

    bool isdead = false;

    public ParticleSystem sink;

    void Start()
    {
        controller = GetComponent<ShipController>();
    }


    void Update()
    {

    }

    public void BeHit(int damage)
    {
        if (isdead) { return; }
        hp = hp < 0 ? 0 : hp - damage;
        if (hp == 0)
        {
            Die();
        }


    }

    void Die()
    {
        isdead = true;
        controller.StateInit();
        StartCoroutine(DeadAnmiation());
        Debug.Log("Die");
    }


    IEnumerator DeadAnmiation()
    {
        yield return new WaitForSeconds(3f);

        ParticleSystem pc= Instantiate(sink, transform.position, Quaternion.identity);
        pc.Play();

        for(int i=0;i<100;i++)
        {
            transform.RotateAround(transform.position, transform.right, -0.5f);
            yield return new WaitForFixedUpdate();
        }
        for(int i=0;i<100;i++)
        {
            transform.position += new Vector3(0, -0.05f, 0);
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }
}
