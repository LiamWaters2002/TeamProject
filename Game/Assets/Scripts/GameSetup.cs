using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    //File
    string folderInRes = "PhotonPrefabs"; //Folder in resources...
    string playerObjectName = "PhotonPlayer"; //Name of player object...


    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine(folderInRes, playerObjectName), Vector2.zero, Quaternion.identity);
    }
}
