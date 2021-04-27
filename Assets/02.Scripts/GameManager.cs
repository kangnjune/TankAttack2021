using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        Vector3 pos = new Vector3(Random.Range(-150.0f, 150.0f), 5.0f, Random.Range(-150.0f,150.0f));

        PhotonNetwork.Instantiate("Tank",new Vector3(0, 5.0f, 0),Quaternion.identity,0);
    }
    
}
