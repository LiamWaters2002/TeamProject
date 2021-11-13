using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonLobby : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonLobby lobby;

    [SerializeField]
    private int roomScene = 1;

    //Player Info
    Player[] players;

    //Textboxes
    [SerializeField]
    private Text lobbyName;
    [SerializeField]
    private Text username;

    //Lobby Buttons
    [SerializeField]
    private GameObject NewRoomButton;
    [SerializeField]
    private GameObject JoinRoomButton;
    [SerializeField]
    private GameObject JoinRandomButton;

    //Room setting constants...
    const int MAX_ROOMS_COUNT= 20;
    const int MAX_PLAYER_COUNT = 4;

    /// <summary>
    /// Creates singleton for the lobby
    /// </summary>
    private void Awake()
    {
        lobby = this;
    }

    /// <summary>
    ///  Connects to Photon using settings in the PhotonnServerSettings file.
    /// </summary>
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// Sync scenes so everyone connected to server is on the same scene
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to the Photon cloud server");
        
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    //public void JoinRoom()
    //{
        //PhotonNetwork.JoinRoom(lobbyName.text);
    //}

    /// <summary>
    /// Create a room using a random number generator for its name.
    /// </summary>
    public void CreateRoom()
    {
        Debug.Log("1");
        int randomRoomNum = Random.Range(1, MAX_ROOMS_COUNT);
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = MAX_PLAYER_COUNT
        };
        Debug.Log("2");
        PhotonNetwork.CreateRoom("Room" + randomRoomNum, roomOptions);//Create room
        Debug.Log("3");
    }

    /// <summary>
    /// If creating the room failed, try to create a new room again.
    /// </summary>
    /// <param name="returnCode">Operation code</param>
    /// <param name="message">Error message</param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Trid to create room but faild, there must already be a room with the same name");
        CreateRoom();
    }

    /// <summary>
    /// Joins a random room.
    /// </summary>
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    /// <summary>
    /// When joining a random server has failed.
    /// </summary>
    /// <param name="returnCode">Operation code</param>
    /// <param name="message">Error message</param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join random game, but failed. Seems like there is no open lobbies avaliable.");
    }

    /// <summary>
    /// Disable callbacks for photon plugin
    /// </summary>
    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    /// <summary>
    /// Callback for when a user successfully joins/creates a room
    /// </summary>
    public override void OnJoinedRoom()
    {
        //PhotonNetwork.NickName = username.text;
        SceneManager.LoadScene(roomScene);
    }

}
