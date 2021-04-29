using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("Room Info")]
    public TMP_Text roomNameText;
    public TMP_Text connectInfoText;
    public TMP_Text messageText;
    [Header("Chatting UI")]

    public TMP_Text chatListText;
    public TMP_InputField msgIF;


    private PhotonView pv;

    public Button exitButton;
    
    //싱글턴 변수
    public static GameManager instance = null;

    void Awake()
    {
        instance = this;

        Vector3 pos = new Vector3(Random.Range(-150.0f, 150.0f), 5.0f, Random.Range(-150.0f,150.0f));

        //통신이 가능한 주인공 캐릭터(탱크) 생성
        PhotonNetwork.Instantiate("Tank",new Vector3(0, 5.0f, 0),Quaternion.identity,0);
    }
    
    void Start()
    {
        SetRoomInfo();
        //pv = GetComponent<PhotonView>(); 아래처럼 선언해도 된다. 
        pv = photonView;
    }

    void SetRoomInfo()
    {
        Room currentRoom = PhotonNetwork.CurrentRoom;
        roomNameText.text = currentRoom.Name;
        connectInfoText.text = $"{currentRoom.PlayerCount}/{currentRoom.MaxPlayers}";
    }
    
    //Exit 버튼 클릭에 연결된 함수
    public void OnExitClick()
    {
        //LeaveRoom 은 호출된다고 바로 빠져나가는게 아니라, 방에 들어온 오브젝트 들을 지우는 작업을 하는 함수다
        //그후 로비에 나가게되면 아래 ONLeftRoom() 함수가 호출된다.
        PhotonNetwork.LeaveRoom();
    }

    // CleanUp 끝난 후에 호출되는 콜백
    public override void OnLeftRoom()
    {
        //Lobby 씬으로 되돌아 가기..
        SceneManager.LoadScene("Lobby");
        //PhotonNetwork.LoadLevel("Lobby");
        //PhotonNetwork 는 굳이 할필요가 없다. Exit 버튼으로 로비로 나와있기때문에 네트워크상에서 Lobby 레벨을 불러올필요가 없기때문 
        //단순 Scene 체인지만 하면서 나올뿐이다. 
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SetRoomInfo();
        string msg = $"\n<color=#00ff00>{newPlayer.NickName}</color> is joined room";
        messageText.text += msg;
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SetRoomInfo();
        // \n = enter 키 값 , 한칸 아래로 내리기위함.
        string msg = $"\n<color=#ff0000>{otherPlayer.NickName}</color> left room";
        messageText.text += msg;
    }

    public void OnSendClick()
    {
        string _msg = $"<color=#00ff00>[{PhotonNetwork.NickName}]</color> {msgIF.text}";
        pv.RPC("SendChatMessage", RpcTarget.AllBufferedViaServer, _msg);
    }

    [PunRPC]
    void SendChatMessage(string msg)
    {
        chatListText.text += $"{msg}\n";
    }

}
