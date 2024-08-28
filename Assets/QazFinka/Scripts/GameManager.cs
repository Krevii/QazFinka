using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon;
using Photon.Pun;
using System.Data;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System;
using Unity.Mathematics;
using System.Globalization;

public class GameManager : MonoBehaviourPunCallbacks
{
    public List<GameObject> players = new List<GameObject>();
    //public TileMapObject tilemap;

    public TextMeshProUGUI pingValue;
    public List<GameObject> profiles = new List<GameObject>();
    public GameObject winPrefab;
    public GameObject losePrefab;

    private PlayerSetup playerSetup;
    private TileMapObject map;
    private CashBook book;
    private PhotonView view;
    private string nickname;



    void Start()
    {
        book = UiManager.GetCanvas().transform.Find("CashBook").GetComponent<CashBook>();
        //book.sliderCashFlow.onValueChanged.AddListener(CheckVictory);
        book.sliderCashFlow.onValueChanged.AddListener(CheckLose);

        playerSetup = FindAnyObjectByType<PlayerSetup>();
        //players = FindObjectsByType<Player>(FindObjectsSortMode.InstanceID);

        view = GetComponent<PhotonView>();

        if (PhotonNetwork.NickName == "")
        {
            PhotonNetwork.NickName = "Player" + UnityEngine.Random.Range(0, 100001);
        }

        nickname = PhotonNetwork.NickName;
        map = FindAnyObjectByType<TileMapObject>();

        //view.RPC("ProfileSetup", RpcTarget.All);
    }

    [PunRPC]
    public void ProfileSetup(List<GameObject> players)
    {
        PhotonView player; //= playerObj.GetComponent<PhotonView>();

        //this.players = players;

        for (int i = 0; i < players.Count; i++)
        {
            player = players[i].GetComponent<PhotonView>();

            Profile profile = profiles[i].GetComponent<Profile>();

            profile.viewId = player.ViewID;

            profile.slider.value = player.gameObject.GetComponent<Player>().sliderVaule;
            profile.slider.maxValue = player.gameObject.GetComponent<Player>().maxSliderVaule;

            //profile.slider = book.sliderCashFlow;
            profile.text.text = player.gameObject.GetComponent<Pawn>().nickname; //"Player" + player.ViewID;

            profiles[i].SetActive(true);


        }
    }
    public void UpdateSlider()
    {

    }
    private void Update()
    {
        pingValue.text = "Ping:" + PhotonNetwork.GetPing().ToString();
    }
    public void CheckVictory(int dreamID, int assetID, int PlayerID)
    {
        if (dreamID == assetID)
        {
            //view.RPC("DeclareVictory", RpcTarget.All, dreamID, assetID, PlayerID);
            view.RPC("DeclareLose", RpcTarget.Others);
            DeclareVictory(dreamID, assetID, PlayerID);

        }
    }
    public void CheckLose(float value)
    {
        if (value < 0 || int.Parse(book.CashPrice.text.Split("าร")[0], NumberStyles.Number) < 0)
        {
            DeclareLose();
        }
    }
    [PunRPC]
    public void DeclareLose()
    {
        GameObject instanceObject = Instantiate(losePrefab, UiManager.GetCanvas().transform);

        GameObject background = instanceObject.transform.Find("Background").gameObject;

        background.transform.Find("Exit Btn").GetComponent<Button>().onClick.AddListener(GotoMainMenu);
        background.transform.Find("Score BG").transform.Find("Score Value").GetComponent<Text>().text = book.sliderCashFlow.value.ToString() + "$";

        PhotonNetwork.LeaveRoom(this);
    }

    [PunRPC]
    public void DeclareVictory(int dreamID, int assetID, int PlayerID)
    {
        GameObject instanceObject = null;

        instanceObject = Instantiate(winPrefab, UiManager.GetCanvas().transform);

        GameObject background = instanceObject.transform.Find("Background").gameObject;

        background.transform.Find("Exit Btn").GetComponent<Button>().onClick.AddListener(GotoMainMenu);
        background.transform.Find("Score BG").transform.Find("Score Value").GetComponent<Text>().text = book.sliderCashFlow.value.ToString() + "$";
        PhotonNetwork.LeaveRoom(this);
    }

    public void GotoMainMenu()
    {
        PhotonNetwork.LeaveRoom(this);
        SceneManager.LoadScene("Menu");

    }


}
