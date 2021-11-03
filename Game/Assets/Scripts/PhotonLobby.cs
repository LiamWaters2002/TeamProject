using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;


    const int MAX_PHOTON = 20;
    const int MAX_LOBBY = 4;

    private void Awake()
    {
        lobby = this; //Creates a singleton
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    //public void onjoinroombuttonclicked()
    //{
    //    photonnetwork.joinroom();
    //}

    public void CreatRoom()
    {
        int randomRoomNum = Random.Range(1, MAX_PHOTON); //Limited to 20 players for photon server.
        RoomOptions roomOpts = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = MAX_LOBBY
        };
        PhotonNetwork.CreateRoom("Lobby" + randomRoomNum, roomOpts);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Trid to create room but faild, there must already be a room with the same name");
        CreatRoom();
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join random game, but failed. Seems like there is no open lobbies avaliable.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("You are now in a room");
    }





    // Update is called once per frame
    void Update()
    {
        
    }
}
