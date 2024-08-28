using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MenuManager : MonoBehaviourPunCallbacks
{
    public InputField NameOfRoomCreate;
    public InputField PassOfRoomCreate;

    public InputField NameOfRoomJoin;
    public InputField PassOfRoomJoin;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;

        PhotonNetwork.NickName = PassOfRoomCreate.text;
        
        PhotonNetwork.CreateRoom(NameOfRoomCreate.text, roomOptions);
    }
    public void JoinRoom()
    {
        PhotonNetwork.NickName = PassOfRoomJoin.text;
        PhotonNetwork.JoinRoom(NameOfRoomJoin.text);

        
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        PhotonNetwork.LoadLevel(1);
    }
}
