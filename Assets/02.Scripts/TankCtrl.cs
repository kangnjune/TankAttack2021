using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Utility;


public class TankCtrl : MonoBehaviour
{

    private Transform tr;
    private Transform cannonTr;
    private Transform turretTr;
    public float moveSpeed = 10.0f;
    private PhotonView pv;
    public AudioClip expSfx;

    public TMPro.TMP_Text userIdText;
    private new AudioSource audio;
    public Transform firePos;
    public GameObject cannon;
    

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
        audio =GetComponent<AudioSource>();
        userIdText.text = pv.Owner.NickName;

        turretTr = tr.Find("Turret").GetComponent<Transform>();
        cannonTr = turretTr.Find("Cannon").GetComponent<Transform>();
        if (pv.IsMine)
        {   
            Camera.main.GetComponent<SmoothFollow>().target = tr.Find("CamPivot").transform;
            GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -5.0f, 0);
            //탱크에 설정된 무게중심을 아래로 내리기 위해 작성
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pv.IsMine)
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");
            float u = Input.GetAxis("Mouse Y");
            float r = Input.GetAxis("Mouse X");

            tr.Translate(Vector3.forward*Time.deltaTime*moveSpeed*v);
            tr.Rotate(Vector3.up*Time.deltaTime*100.0f*h);
            turretTr.Rotate(Vector3.up *Time.deltaTime*100.0f *r);
            cannonTr.Rotate(Vector3.right * Time.deltaTime * 100.0f * u);
            //포탄 발사 로직
            if (Input.GetMouseButtonDown(0))
            {
                pv.RPC("Fire",RpcTarget.AllViaServer, pv.Owner.NickName); 
                //AllViaServer = 레이턴시 때문에 서로 포탄이 미스매치 되어 보이는데, 그걸 조금 줄여주는 기능
                //서버에 맞춰서 함수를 호출해주기때문
            }
        }
    }
    [PunRPC]
    void Fire(string shooterName)
    {
        audio?.PlayOneShot(expSfx);
        GameObject _cannon = Instantiate(cannon, firePos.position , firePos.rotation);
        _cannon.GetComponent<Cannon>().shooter = shooterName;
    }
}
