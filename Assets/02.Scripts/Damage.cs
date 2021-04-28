using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Damage : MonoBehaviour
{
    private List<MeshRenderer> renderers = new List<MeshRenderer>();
    public int hp = 100;
    private PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        GetComponentsInChildren<MeshRenderer>(renderers);
        //모든 메시렌더러 List를 가져오는것이니까.
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.CompareTag("CANNON"))
        {
            string shooter = coll.gameObject.GetComponent<Cannon>().shooter;
            hp -= 10;
            if (hp <=0)
            {
                StartCoroutine(TankDestrtoy(shooter)); 
                //그냥 넘겨주려니까 위에 선언한 string shooter 가 안넘어가져서 파라미터로 넘겨줘야한다.
            }
        }
    }

    IEnumerator TankDestrtoy(string shooter)
    {
        //렌더러 컴포넌트에서 비활성화해서 안보이게 한후 5초동안 기다리고 다시 렌더러컴포넌트 활성화 해서 등장!
        string msg = $"\n<color=#00ff00>{pv.Owner.NickName}</color> is killed by <color=#ff0000>{shooter}</color>";
        GameManager.instance.messageText.text += msg;
        //발사로직을 정지
        //렌더러 컴포넌트 , RigidBody 비활성화
        GetComponent<BoxCollider>().enabled =false;
        if (pv.IsMine)
        {
        GetComponent<Rigidbody>().isKinematic = true;
        }
        foreach(var mesh in renderers) mesh.enabled = false;

        //5초 waiting
        yield return new WaitForSeconds(5.0f);
        //불규칙한 위치로 이동. 같은장소에서 리스폰되면 안되니까
        Vector3 pos = new Vector3 (Random.Range(-150.0f,150.0f), 5.0f, Random.Range(-150.0f,150.0f));
        transform.position = pos;

        //렌더러 컴포넌트  , RigidBody 활성화
        hp = 100;
        GetComponent<BoxCollider>().enabled = true;
        if (pv.IsMine)
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
        foreach(var mesh in renderers) mesh.enabled = true;

    }

}
