using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private static NetworkManager instance = null;

    GameObject InGameManager;

    public GameObject GameStartUI;
    public GameObject GameLobbyUI;
    public GameObject GameSelectUI;

    [Header("일반게임UI")]
    public GameObject NormalgameUI;
    public GameObject NormalCreateUI;
    public GameObject NormalJoinUI;

    [Header("경쟁게임UI")]
    public GameObject CPTgameUI;
    public GameObject CPTCreateUI;
    public GameObject CPTJoinUI;

    [Header("아케이드게임UI")]


    public TMP_InputField RoomInput;
    /*
    [Header("DisconnectPanel")]
    public InputField NickNameInput;

    [Header("LobbyPanel")]
    public GameObject LobbyPanel;
    public Text WelcomeText;
    public Text LobbyInfoText;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;

    [Header("RoomPanel")]
    public GameObject RoomPanel;
    public Text ListText;
    public Text RoomInfoText;
    
    */
    [Header("Chat")]
    TextMeshProUGUI[] ChatText;
    TMP_InputField ChatInput;

    [Header("ETC")]
    public TextMeshProUGUI StatusText;
    public PhotonView PV;

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;

    void Awake()
    {
        Screen.SetResolution(960, 540, false);
        /*if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }*/

        Connect();
    }
    void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        //LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비 / " + PhotonNetwork.CountOfPlayers + "접속";

    }

    #region 방리스트 갱신
    // ◀버튼 -2 , ▶버튼 -1 , 셀 숫자
    public void MyListClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        MyListRenewal();
    }

    void MyListRenewal()
    {
        // 최대페이지
        /*maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        // 이전, 다음버튼
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        }*/
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }
    #endregion


    #region 서버연결
    

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        //PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        //WelcomeText.text = PhotonNetwork.LocalPlayer.NickName + "님 환영합니다";
        GameStartUI.SetActive(false);
        GameLobbyUI.SetActive(true);
        myList.Clear();
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        //LobbyPanel.SetActive(false);
        //RoomPanel.SetActive(false);
    }
    #endregion



    #region 방
    public void CreateRoom() => PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Room" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 2 });
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnJoinedRoom()
    {
        //RoomPanel.SetActive(true);
        //RoomRenewal();
        //ChatInput.text = "";
        //for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";

        print("방입장");
        SceneManager.LoadScene("GameScene");
    }

    public override void OnCreateRoomFailed(short returnCode, string message) => CreateRoom();

    public override void OnJoinRandomFailed(short returnCode, string message) => CreateRoom();

    /*public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //RoomRenewal();
        ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    }*/

    /*void RoomRenewal()
    {
        ListText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "명 / " + PhotonNetwork.CurrentRoom.MaxPlayers + "최대";
    }*/
    #endregion

    public void SelectUIBack()
    {
        GameLobbyUI.SetActive(true);
        GameSelectUI.SetActive(false);
    }

    #region 일반게임
    public void NorMalGameUI() => NormalgameUI.SetActive(true);
    public void NorMalGameUIClose() => NormalgameUI.SetActive(false);
    public void NormalCreateUIOpen() => NormalCreateUI.SetActive(true);
    public void NormalNotCreate() => NormalCreateUI.SetActive(false);
    public void NormalJoinUIOpen() => NormalJoinUI.SetActive(true);
    public void NormalJoinUIClose() => NormalJoinUI.SetActive(false);
    #endregion


    #region 경쟁게임
    public void CPTGameUI() => CPTgameUI.SetActive(true);
    public void CPTGameUIClose() => CPTgameUI.SetActive(false);
    public void CPTCreateUIOpen() => CPTCreateUI.SetActive(true);
    public void CPTNotCreate() => CPTCreateUI.SetActive(false);
    public void CPTJoinUIOpen() => CPTJoinUI.SetActive(true);
    public void JoinUIClose() => CPTJoinUI.SetActive(false);
    #endregion


    public void GameSelectUIOpen()
    {
        GameLobbyUI.SetActive(false);
        GameSelectUI.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}