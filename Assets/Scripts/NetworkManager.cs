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

    [Header("�Ϲݰ���UI")]
    public GameObject NormalgameUI;
    public GameObject NormalCreateUI;
    public GameObject NormalJoinUI;

    [Header("�������UI")]
    public GameObject CPTgameUI;
    public GameObject CPTCreateUI;
    public GameObject CPTJoinUI;

    [Header("�����̵����UI")]


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
        //LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "�κ� / " + PhotonNetwork.CountOfPlayers + "����";

    }

    #region �渮��Ʈ ����
    // ����ư -2 , ����ư -1 , �� ����
    public void MyListClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        MyListRenewal();
    }

    void MyListRenewal()
    {
        // �ִ�������
        /*maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        // ����, ������ư
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // �������� �´� ����Ʈ ����
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


    #region ��������
    

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        //PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        //WelcomeText.text = PhotonNetwork.LocalPlayer.NickName + "�� ȯ���մϴ�";
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



    #region ��
    public void CreateRoom() => PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Room" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 2 });
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnJoinedRoom()
    {
        //RoomPanel.SetActive(true);
        //RoomRenewal();
        //ChatInput.text = "";
        //for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";

        print("������");
        SceneManager.LoadScene("GameScene");
    }

    public override void OnCreateRoomFailed(short returnCode, string message) => CreateRoom();

    public override void OnJoinRandomFailed(short returnCode, string message) => CreateRoom();

    /*public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //RoomRenewal();
        ChatRPC("<color=yellow>" + newPlayer.NickName + "���� �����ϼ̽��ϴ�</color>");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "���� �����ϼ̽��ϴ�</color>");
    }*/

    /*void RoomRenewal()
    {
        ListText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "�� / " + PhotonNetwork.CurrentRoom.MaxPlayers + "�ִ�";
    }*/
    #endregion

    public void SelectUIBack()
    {
        GameLobbyUI.SetActive(true);
        GameSelectUI.SetActive(false);
    }

    #region �Ϲݰ���
    public void NorMalGameUI() => NormalgameUI.SetActive(true);
    public void NorMalGameUIClose() => NormalgameUI.SetActive(false);
    public void NormalCreateUIOpen() => NormalCreateUI.SetActive(true);
    public void NormalNotCreate() => NormalCreateUI.SetActive(false);
    public void NormalJoinUIOpen() => NormalJoinUI.SetActive(true);
    public void NormalJoinUIClose() => NormalJoinUI.SetActive(false);
    #endregion


    #region �������
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