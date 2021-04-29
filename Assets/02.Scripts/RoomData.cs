using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomData : MonoBehaviour
{
    private TMP_Text roomInfoText;
    private RoomInfo _roomInfo;

//프로퍼티
    public RoomInfo RoomInfo
    {
        get
        {
            return _roomInfo;
        }
        set
        {
            _roomInfo = value;
            // room_03 (12/30) 이런식으로 표현하겠다는 뜻
            roomInfoText.text = $"{_roomInfo.Name} ({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";
            //버튼의 클릭 이벤트 함수 연결
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener( () =>OnEnterRoom(_roomInfo.Name) );
            //onClick 이 클릭이벤트 연결해주는 기능, AddListener 가 우리가 추가해줬던 기능을 추가한다는뜻

            //AddListener 뒤에 파라미터엔 람다식으로 사용가능(원래는 델리게이트 사용(람다식은 델리게이트를 아주 간단하게 줄인 문법))
            //GetComponent<UnityEngine.UI.Button>().onClick.AddListener( delegate () { OnEnterRoom(_roomInfo.Name);} ); 
        }
    }

    void Awake()
    {
        roomInfoText = GetComponentInChildren<TMP_Text>();
    }

    void OnEnterRoom(string roomName)
    {
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 30;

        PhotonNetwork.JoinOrCreateRoom(roomName ,ro ,TypedLobby.Default);
    }
}
