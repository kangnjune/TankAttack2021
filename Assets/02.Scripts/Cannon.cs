using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    
    public float speed = 2000.0f;
    public GameObject expEffect;
    public string shooter;

    void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward*speed);
    }
    
    void OnCollisionEnter(Collision coll)
    {

        GameObject obj = Instantiate(expEffect, transform.position , Quaternion.identity);
        Destroy(obj, 3.0f);
        Destroy(this.gameObject);
        /*
        ContactPoint cont = coll.GetContact(0);
        
        if(coll.collider.gameObject)
        {
            GameObject exp = Instantiate(expEffect, cont.normal , Quaternion.LookRotation(-cont.normal));
        }
        */
    }
}
