using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Billboard : MonoBehaviour
{
    private Transform camTr;

    // Start is called before the first frame update
    void Start()
    {
        camTr = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(camTr.position);
    }
}
