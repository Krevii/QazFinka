using Photon.Pun.Demo.PunBasics;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System.Linq;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public List<GameObject> players = new List<GameObject>();
    public List<TileObject> tileObjects;
    public TileMapObject tileMap;

    [SerializeField]
    private GameObject player;
    private PhotonView view;
    private GameManager gameManager;
    //private PhotonView view;

    void Start()
    {
        tileMap = FindAnyObjectByType<TileMapObject>();
        tileObjects = tileMap.tileObjects;
        gameManager = FindAnyObjectByType<GameManager>();
        view = GetComponent<PhotonView>();
        InitializePlayers();
    }

    private void InitializePlayers()
    {

        Vector3 startPosition = tileObjects[0].transform.position;


        player = PhotonNetwork.Instantiate("Pawn", startPosition, Quaternion.identity);

        //Debug.LogError("player:" + player.GetComponent<PhotonView>().ViewID + "\n" + "isMine:" + player.GetComponent<PhotonView>().IsMine);

        player.transform.position = startPosition;

        player.GetComponent<Pawn>().skin = "skins/Player_" + Random.Range(1, 5);
        player.GetComponent<Pawn>().nickname = PhotonNetwork.NickName;

        view.RPC("PlayerList", RpcTarget.All);
    }

    [PunRPC]
    public IEnumerator PlayerList()
    {
        yield return new WaitForSeconds(0.1f);
        players.Clear();

        Player[] playersTemp = FindObjectsByType<Player>(FindObjectsSortMode.InstanceID);

        foreach (var item in playersTemp)
        {
            players.Add(item.gameObject);
        }

        players = players.OrderBy(p => p.GetComponent<PhotonView>().ViewID).ToList();
        
        gameManager.ProfileSetup(players);

    }
}
