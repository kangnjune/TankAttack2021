using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Utility;


public class TankCtrl : MonoBehaviour , IPunObservable
{

    private Transform tr;
    public float moveSpeed = 10.0f;
    public Transform cannonMesh;
    public Transform firePos;
    public GameObject cannon;

    private PhotonView pv;
    public AudioClip expSfx;
    public TMPro.TMP_Text userIdText;
    private new AudioSource audio;
    

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
        audio =GetComponent<AudioSource>();
        userIdText.text = pv.Owner.NickName;

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

            tr.Translate(Vector3.forward*Time.deltaTime*moveSpeed*v);
            tr.Rotate(Vector3.up*Time.deltaTime*100.0f*h);

            //포탄 발사 로직
            if (Input.GetMouseButtonDown(0))
            {
                pv.RPC("Fire",RpcTarget.AllViaServer, pv.Owner.NickName); 
                //AllViaServer = 레이턴시 때문에 서로 포탄이 미스매치 되어 보이는데, 그걸 조금 줄여주는 기능
                //서버에 맞춰서 함수를 호출해주기때문
            }
            // 포신 회전 설정
            float r = Input.GetAxis("Mouse ScrollWheel");
            cannonMesh.Rotate(Vector3.right * Time.deltaTime * r * 100.0f);
        }
        else
        {
            //레이턴시에 따른 문제가 있을수 있어서 
            if((tr.position - receivePos).sqrMagnitude > 3.0f*3.0f)
            {
                tr.position = receivePos;
            }
            else
            {
                tr.position = Vector3.Lerp(tr.position, receivePos, Time.deltaTime*10.0f);
            }

            tr.rotation = Quaternion.Slerp(tr.rotation, receiveRot,Time.deltaTime*10.0f);
        }
    }
    [PunRPC]
    void Fire(string shooterName)
    {
        audio?.PlayOneShot(expSfx);
        GameObject _cannon = Instantiate(cannon, firePos.position , firePos.rotation);
        _cannon.GetComponent<Cannon>().shooter = shooterName;
    }

    //네트워크를 통해서 수신받을 변수
    Vector3 receivePos = Vector3.zero;              
    Quaternion receiveRot = Quaternion.identity;
//기존 Photon View 로 처리했던 부분을 , 커스텀하게 옵션을 주고싶다면 , 이 함수를 쓰면된다.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting) //IsWritting 의 뜻은 photonView가 IsMine == true 일때 
        {
            stream.SendNext(tr.position); //위치값 보냄
            stream.SendNext(tr.rotation); //회전값 보냄
        }
        else
        {
            //타입 변환을 해줘야한다.
            receivePos = (Vector3) stream.ReceiveNext();
            receiveRot = (Quaternion) stream.ReceiveNext();
        }
    }
}
