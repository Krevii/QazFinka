using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using Photon.Pun.Demo.PunBasics;
using System;

public class TileMapObject : MonoBehaviour
{
    private GameObject cashbook;
    private PhotonView currentPlayerview;
    private PhotonView view;
    private string cashText;

    [SerializeField]
    private int currentPlayerIndex = 0;
    public List<GameObject> players = new List<GameObject>();
    public GameObject playerPawnPrefab;
    public GameObject playerPawnPrefab2;
    public GameObject currentPlayer = null;
    public Dice dice;
    public GameObject MapOut;
    public List<TileObject> tileObjects;
    public List<TileObject> tileObjectsOut;
    private List<int> playerPositions = new List<int>(); // Список для хранения позиций каждого игрока
    private GameObject diceRollButton;
    private PlayerManager playerManager; // Ссылка на PlayerManager

    private void Awake()
    {
        tileObjects.AddRange(gameObject.GetComponentsInChildren<TileObject>());
        tileObjectsOut.AddRange(MapOut.GetComponentsInChildren<TileObject>());
    }

    private void Start()
    {
        playerManager = new PlayerManager(); // Создаем экземпляр PlayerManager
        view = GetComponent<PhotonView>();
        dice = FindFirstObjectByType<Dice>();
        diceRollButton = GameObject.Find("Dice Roll Button");

        if (view.AmOwner)
        {
            diceRollButton.SetActive(true);
        }
        else
        {
            diceRollButton.SetActive(false);
        }
    }

    [PunRPC]
    public void PlayerSort()
    {
        players.Clear();

        Player[] temp = FindObjectsByType<Player>(FindObjectsSortMode.None);


        foreach (var item in temp)
        {
            players.Add(item.gameObject);
            //playerPositions.Add(0);
        }

        players = players.OrderBy(p => p.GetComponent<PhotonView>().ViewID).ToList();
    }
    [PunRPC]
    public void RollDiceRPC()
    {
        int _diceResult = dice.DiceRoll();
        //view.RPC("PlayerSort", RpcTarget.All);
        view.RPC("RollDice", RpcTarget.All, _diceResult);


    }
    [PunRPC]
    public void RollDice(int diceResult)
    {

        players.Clear();

        Player[] temp = FindObjectsByType<Player>(FindObjectsSortMode.InstanceID);

        foreach (var item in temp)
        {
            players.Add(item.gameObject);
            playerPositions.Add(0);
        }

        players = players.OrderBy(p => p.GetComponent<PhotonView>().ViewID).ToList();


        Player playerToOutCircle = players[currentPlayerIndex].GetComponent<Player>();
        
        if (playerToOutCircle.isCashflow)
        {
            StartCoroutine(WalkerOnBoardOut(diceResult));   
        }
        else
        {
            StartCoroutine(WalkerOnBoard(diceResult));
        }

    }

    private IEnumerator WalkerOnBoardOut(int diceResult)
    {
        currentPlayer = players[currentPlayerIndex];
        int currentIndex = playerPositions[currentPlayerIndex]; // Где стоит ебаная пешка

        currentPlayerview = currentPlayer.GetComponent<PhotonView>();

        Player firstPlayer = currentPlayer.GetComponent<Player>();

        if (currentPlayerview.IsMine)
        {
            diceRollButton.SetActive(true);
        }
        else
        {
            diceRollButton.SetActive(false);
        }


        for (int j = 0; j < diceResult; j++)
        {
            currentIndex++;
            currentIndex %= tileObjectsOut.Count;

            Vector3 diceToPointDiceResult = tileObjectsOut[currentIndex].transform.position;
            currentPlayer.GetComponent<Pawn>().MovePawn(diceToPointDiceResult);

            if (currentPlayerview.IsMine)
            {
                firstPlayer.PayDayCheck(tileObjectsOut[currentIndex]);
            }

            tileObjectsOut[currentIndex].WaveTileAnimation();
            yield return new WaitForSeconds(0.2f);
        }

        playerPositions[currentPlayerIndex] = currentIndex; // Сохраняем текущую позицию ебучего игрока

        if (currentPlayerview.IsMine)
        {
            if (firstPlayer != null)
            {
                firstPlayer.gameObject.GetComponent<Pawn>().TileCheck(tileObjectsOut[currentIndex]);
            }
        }

        IncrementPlayerIndex();

        if (players[currentPlayerIndex].GetComponent<PhotonView>().IsMine)
        {
            diceRollButton.SetActive(true);
        }
        else
        {
            diceRollButton.SetActive(false);
        }
    }

    private IEnumerator WalkerOnBoard(int diceResult)
    {

        currentPlayer = players[currentPlayerIndex];
        int currentIndex = playerPositions[currentPlayerIndex]; // Где стоит ебаная пешка

        currentPlayerview = currentPlayer.GetComponent<PhotonView>();

        Player firstPlayer = currentPlayer.GetComponent<Player>();

        if (currentPlayerview.IsMine)
        {
            diceRollButton.SetActive(true);
        }
        else
        {
            diceRollButton.SetActive(false);
        }


        for (int j = 0; j < diceResult; j++)
        {
            currentIndex++;
            currentIndex %= tileObjects.Count;

            Vector3 diceToPointDiceResult = tileObjects[currentIndex].transform.position;
            currentPlayer.GetComponent<Pawn>().MovePawn(diceToPointDiceResult);

            if (currentPlayerview.IsMine)
            {
                firstPlayer.PayDayCheck(tileObjects[currentIndex]);
            }

            tileObjects[currentIndex].WaveTileAnimation();
            yield return new WaitForSeconds(0.2f);
        }

        playerPositions[currentPlayerIndex] = currentIndex; // Сохраняем текущую позицию ебучего игрока

        if (currentPlayerview.IsMine)
        {
            if (firstPlayer != null)
            {
                firstPlayer.gameObject.GetComponent<Pawn>().TileCheck(tileObjects[currentIndex]);
            }
        }

        IncrementPlayerIndex();

        if (players[currentPlayerIndex].GetComponent<PhotonView>().IsMine)
        {
            diceRollButton.SetActive(true);
        }
        else
        {
            diceRollButton.SetActive(false);
        }
    }

    private void IncrementPlayerIndex()
    {

        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Count)
        {
            currentPlayerIndex = 0;
        }
    }
}



