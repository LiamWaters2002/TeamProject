using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    public void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Vector3 position = new Vector3(8, 8, -1);
            PhotonNetwork.Instantiate(playerPrefab.name, position, Quaternion.identity);
        }
        else
        {
            Vector3 position = new Vector3(80, 8, -1);
            PhotonNetwork.Instantiate(playerPrefab.name, position, Quaternion.identity);
        }
        
    }
}
