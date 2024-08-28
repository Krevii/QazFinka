using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TileMapObject tileMapObject = FindFirstObjectByType<TileMapObject>();

        if (GetComponent<PhotonView>().IsMine && tileMapObject == null)
        {
            tileMapObject = PhotonNetwork.Instantiate("Map",Vector3.zero,Quaternion.identity).GetComponent<TileMapObject>();
        }

        //tileMapObject.players.Add(FindObjectsByType<Player>(FindObjectsSortMode.InstanceID)[1].gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
