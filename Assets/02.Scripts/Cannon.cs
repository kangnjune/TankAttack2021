using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private GameObject cannon;
    public float speed = 2000.0f;
    void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward*speed);
    }
}
