using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "v1.0";
    private string userId = "NaruHoya";

    public TMP_InputField userIdText;
    public TMP_InputField roomNameText;

    void Awake()
    {   
        //방장이 Scene을 로딩하게되면 자동적으로 참가자들에게도 같은 Scene을 출력하는 기능
        PhotonNetwork.AutomaticallySyncScene = true;
        //게임 버전 지정
        PhotonNetwork.GameVersion = gameVersion;
        //유저명 지정
        //PhotonNetwork.NickName = userId;

        //서버 접속 ping test
        PhotonNetwork.ConnectUsingSettings();
        //이걸 하면 OnconnectedToMaster() 함수로 들어가게된다.

    }

    void Start()
    {
        userId = PlayerPrefs.GetString("User_ID", $"USER_{Random.Range(0,100):00}");
        userIdText.text = userId;
        PhotonNetwork.NickName = userId;
    }
    //포톤 서버로 접속됨을 알려주는 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Server!!!");
        //PhotonNetwork.JoinRandomRoom(); //만들어진 아무 방에 접속 시도
        
        //로비라는 공간으로 먼저 접속을 해야한다.
        //예전엔 로비를 무조건 갔는데, 현재는 인스턴스게임같은경우 바로 만나서 겜하면 되는경우가 있어서 필요한 사람만 
        //로비를 입장하게 기능을 바꿈(패치함)
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby!!!");
    }

//랜덤 방에 들어가는걸 실패하면 실행되는 함수 
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"code={returnCode},msg = {message}");
        // 방의 옵션 설정
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 30;
        // 방을 생성
        PhotonNetwork.CreateRoom(roomNameText.text, ro);
    }

    // 방 생성 완료 콜백함수
    public override void OnCreatedRoom()
    {
        Debug.Log("방생성 완료");
    }

    //방에 입장했을 때 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 완료");
        Debug.Log(PhotonNetwork.CurrentRoom.Name);

        //유니티 LoadScene 과 같은 기능, 다만 유니티의 LoadScene을 쓰면 
        //스크립트로 서버접속을 잠시 끊고, 씬 불러오고 다시 연결하는 코드를 써야하는데.
        //PhotonNetwork.LoadLevel() 을 쓰면 그 작업을 자동으로 해주기때문에 이걸 써야한다.
        
        //Awake() 함수에서 PhotonNetwork.AutomaticallySyncScene = true; 를 사용했고, 
        //그에 따라서 마스터일때만 Scene 을 Load 해야하니까, 아래 조건을 넣어줘야한다.
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("BattleField");
        }
        // 통신이 가능한 주인공 캐릭터(탱크) 생성
        // 똑같이 생성하는 함수지만 network 를 통해 다른 유저들에게도 탱크르 생성하라고 명령을 보내는 함수
        //PhotonNetwork.Instantiate("Tank",new Vector3(0, 5.0f, 0),Quaternion.identity,0); 
        //마지막 0 은 그룹아이디 인데 그룹아이디가 다르면 안보인다

        //Lobby 생성하는 작업을 하고 나서 Scene 을 분리하고 주석처리함,
        //게임매니저 스크립트에서 탱크를 생성하게 추가할것.
    }
    //룸 목록 수신
    public override void OnRoomListUpdate(List<RoomInfo> roomlist)
    {
        foreach (var room in roomlist)
        {
            Debug.Log($"room name={room.Name}, ({room.PlayerCount}/{room.MaxPlayers})");
        }
    }




#region UI_BUTTON_CALLBACK
    //로그인 버튼 클릭하면 랜덤한 방에 들어가지는 함수
    public void OnLoginClick()
    {
        if(string.IsNullOrEmpty(userIdText.text))
        {
            userId = $"USER_{Random.Range(0,100):00}";
            userIdText.text = userId;
        }

        PlayerPrefs.SetString("USER_ID", userIdText.text);
        PhotonNetwork.NickName = userIdText.text;
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnMakeRoomClick()
    {
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 30;

        if(string.IsNullOrEmpty(roomNameText.text))
        {
            roomNameText.text = $"Room_{Random.Range(0,100):000}";  
        }
        // 방을 생성
        PhotonNetwork.CreateRoom(roomNameText.text, ro);
    }
#endregion
}


