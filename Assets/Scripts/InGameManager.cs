using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class InGameManager : MonoBehaviour
{
    public PhotonView inGamePV;

    public GameObject CharSelectUI;
    public GameObject TabUI;

    public string[] CharPrefabs = new string[7];
    public Transform[] CharSpawnPos = new Transform[6];

    [HideInInspector]
    public GameObject PlayerObj;

    [Header("Chat")]
    NetworkManager NetManager = new NetworkManager();
    public TextMeshProUGUI[] ChatText;
    public TMP_InputField ChatInput;
    public GameObject[] ChatObj = new GameObject[2];
    bool isChat = false;

    int charNum;
    bool charSelect = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TabUIOpen();
        HUIOpen();
        ChatOpen();
    }
    void ChatOpen()
    {
        if (!isChat)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                for (int i = 0; i < 2; i++) ChatObj[i].SetActive(true);
                isChat = true;
                ChatInput.ActivateInputField();
            }
        }
        else if(isChat)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                Send();
                ChatObj[1].SetActive(false);
                isChat = false;
            }
        }
    }
    #region 채팅
    public void Send()
    {
        inGamePV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text);
        ChatInput.text = "";
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++)
            if (ChatText[i].text == "")
            {
                isInput = true;
                ChatText[i].text = msg;
                break;
            }
        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
        }
    }
    #endregion
    void TabUIOpen()
    {
        if (charSelect)
        {
            if (Input.GetKey(KeyCode.Tab)) TabUI.SetActive(true);
            else if (Input.GetKeyUp(KeyCode.Tab)) TabUI.SetActive(false);
        }
    }
    void HUIOpen()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            charSelect = false;
            CharSelectUI.SetActive(true);
        }        
    }
    public void CharacterSelect(int CharNum)
    {
        charNum = CharNum;
    }
    public void CharClose()
    {
        charSelect = true;
        CharSelectUI.SetActive(false);
        PlayerObj = PhotonNetwork.Instantiate(CharPrefabs[charNum], CharSpawnPos[charNum].position, Quaternion.identity);

        if (PhotonNetwork.CurrentRoom.PlayerCount % 2 == 0)
        {
            PlayerObj.tag = "RedTeam";
        }
        else PlayerObj.tag = "BlueTeam";

        print("생성");
    }
}