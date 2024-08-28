using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class PlayerManager
{
    public List<PlayerData> players = new List<PlayerData>();

    public void AddPlayer(string playerName)
    {
        PlayerData newPlayer = new PlayerData();
        newPlayer.playerName = playerName;
        newPlayer.playerBalance = 1000f;
        players.Add(newPlayer);

    }

   
}

