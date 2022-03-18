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
    private int roomScene = 2;

    //Player Info
    Player[] players;

    //Popups
    [SerializeField]
    public GameObject leaderboard;
    [SerializeField]
    public GameObject credits;
    [SerializeField]
    public GameObject creditsPage1;
    [SerializeField]
    public GameObject creditsPage2;
    [SerializeField]
    public GameObject popupBox;
    [SerializeField]
    public Button exitPopupBtn;
    [SerializeField]
    public Text errorMessage;


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
    const int MAX_ROOMS_COUNT = 20;
    const int MAX_PLAYER_COUNT = 4;

    /// <summary>
    /// Creates singleton for the lobby
    /// </summary>
    private void Awake()
    {
        lobby = this;
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    ///  Connects to Photon using settings in the PhotonnServerSettings file.
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Sync scenes so everyone connected to server is on the same scene
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to the Photon cloud server");
        PhotonNetwork.JoinLobby();//new
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void JoinRoom()
    {
        PhotonNetwork.NickName = username.text;
        PhotonNetwork.JoinRoom(lobbyName.text);
    }

    /// <summary>
    /// Create a room using a random number generator for its name.
    /// </summary>
    public void CreateRoom()
    {
        if (username.text != "")
        {
            RoomOptions roomOptions = new RoomOptions()
            {
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = MAX_PLAYER_COUNT
            };
            if (lobbyName.text == "")
            {
                int randomRoomNum = Random.Range(1, MAX_ROOMS_COUNT);
                PhotonNetwork.CreateRoom("Room" + randomRoomNum, roomOptions);//Create room
            }
            else
            {
                PhotonNetwork.CreateRoom(lobbyName.text, roomOptions);//Create room
            }
        }
        else
        {
            errorMessage.text = "Please enter a username before creating / joining a room.";
            popupBox.SetActive(true);
        }


    }

    /// <summary>
    /// If creating the room failed, try to create a new room again.
    /// </summary>
    /// <param name="returnCode">Operation code</param>
    /// <param name="message">Error message</param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorMessage.text = "Failed to create room due to: Identical name to an existing room. Try a different name.";
        popupBox.SetActive(true);
    }

    public void exitPopup()
    {
        if (popupBox.activeSelf)
        {
            popupBox.SetActive(false);
        }
        if (leaderboard.activeSelf)
        {
            leaderboard.SetActive(false);
        }
        if (credits.activeSelf)
        {
            credits.SetActive(false);
        }
        
    }

    /// <summary>
    /// Joins a random room.
    /// </summary>
    public void JoinRandomRoom()
    {
        if (username.text != "")
        {
            PhotonNetwork.NickName = username.text;
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            errorMessage.text = "Please enter a username before creating / joining a room.";
            popupBox.SetActive(true);
        }

    }
    /// <summary>
    /// When joining a random server has failed.
    /// </summary>
    /// <param name="returnCode">Operation code</param>
    /// <param name="message">Error message</param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        errorMessage.text = "Tried to join random game, but failed. Seems like there is no open lobbies avaliable.";
        popupBox.SetActive(true);
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
        if (username.text != "")
        {
            PhotonNetwork.NickName = username.text;
            SceneManager.LoadScene(roomScene);
        }
        else
        {
            errorMessage.text = "Please enter a username before creating / joining a room.";
            popupBox.SetActive(true);
        }

    }

    public void toggleLeaderboard()
    {
        if (leaderboard.activeSelf)
        {
            leaderboard.active = false;
        }    
        else
        {
            leaderboard.active = true;
        }
    }

    public void toggleCredits()
    {
        if (credits.activeSelf)
        {
            credits.active = false;
        } 
        else
        {
            credits.active = true;
        }
    }

}


        