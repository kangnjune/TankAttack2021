using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Turret : MonoBehaviour
{
    private PhotonView pv;

    void Start()
    {
        pv =GetComponent<PhotonView>();
        this.enabled = pv.IsMine;
    }

    // Update is called once per frame
    void Update()
    {
        float r = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up*Time.deltaTime*r*200.0f);
    }
}
