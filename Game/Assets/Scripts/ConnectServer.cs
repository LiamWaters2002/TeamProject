using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectServer : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// Allows users to connect to the database based on their game version.
    /// </summary>
    void Start()
    {
        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// Overrides method, prints message stating user has connected.
    /// </summary>
    public override void OnConnectedToMaster()
    {
        print("Connected to server");
    }

    /// <summary>
    /// Overrides method, prints message stating user has disconnected, with cause.
    /// </summary>
    /// <param name="cause">Object which contains the cause of the disconnection</param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected from server due to:" + cause);
    }
}
