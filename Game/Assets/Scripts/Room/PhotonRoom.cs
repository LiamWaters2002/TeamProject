using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    //Room info
    public static PhotonRoom room;
    private PhotonView view;
    public int roomSize;
    [SerializeField]
    public int minPlayersToStart;
    public int playerCount;

    //buttons
    [SerializeField]
    private GameObject StartBtn;
    [SerializeField]
    private GameObject Load;
    private GameObject Leave;

    //Scenes
    [SerializeField]
    private int lobbyScene = 0;
    [SerializeField]
    private int gameScene = 2;
       
    //Text variables for display
    [SerializeField]
    private Text playerCountDisplay;
    [SerializeField]
    private Text timerToStartDisplay;
    [SerializeField]
    private Text roomName;

    //boolean values for if timer can start to count down
    private bool startCountDown;
    private bool gameReady;
    private bool startingGame;

    //Countdown timer variables
    private float timerToStartGame;
    private float notFullGameTimer;
    private float fullGameTimer;
    [SerializeField]
    private float maxWaitTime;
    [SerializeField]
    private float maxFullGameWaitTime;

    //Map Selector Popup
    public GameObject mapSelector;

    private void Awake()
    {
        roomName.text = "Room: " + PhotonNetwork.CurrentRoom.Name;

        view = GetComponent<PhotonView>();
        fullGameTimer = maxFullGameWaitTime;
        notFullGameTimer = maxWaitTime;
        timerToStartGame = maxWaitTime;

        PlayerCountUpdate();
    }

    /// <summary>
    /// Detmine how ready the room is based on playercount and maximum playercount within the room.
    /// </summary>
    void PlayerCountUpdate()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        playerCountDisplay.text = playerCount + "/" + roomSize;

        if (playerCount == roomSize)
        {
            gameReady = true;
        }
        else if (playerCount >= minPlayersToStart)
        {
            startCountDown = true;
        }
        else
        {
            startCountDown = false;
            gameReady = false;
        }
    }
    /// <summary>
    /// Send a new player the current time.
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerCountUpdate();
        if (PhotonNetwork.IsMasterClient)
        {
            view.RPC("RPCtimer", RpcTarget.Others, startCountDown);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="timeIn"></param>
    [PunRPC]
    private void RPCtimer(float timeIn)
    {
        timerToStartGame = timeIn;
        notFullGameTimer = timeIn;
        if (timeIn < fullGameTimer)
        {
            fullGameTimer = timeIn;
        }
    }

    public override void OnPlayerLeftRoom(Player leftPlayer)
    {
        PlayerCountUpdate();
    }

    /// <summary>
    /// Enable callbacks for photon plugin
    /// </summary>
    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
    }

    // Update is called once per frame
    void Update()
    {
        WaitingForMorePlayers();
    }

    /// <summary>
    /// set timer to maxWaitTime if a minimum amount of players is not met.
    /// </summary>
    void ResetTimer()
    {
        timerToStartGame = maxWaitTime;
        notFullGameTimer = maxWaitTime;
        fullGameTimer = maxFullGameWaitTime;
    }

    /// <summary>
    /// Determine the length of the countdown depending on the playercount.
    /// </summary>
    void WaitingForMorePlayers()
    {
        if(playerCount <= 1)
        {
            StartBtn.SetActive(false);
            ResetTimer();
        }

        if (gameReady)
        {
            StartBtn.SetActive(true);
            fullGameTimer -= Time.deltaTime;
            timerToStartGame = fullGameTimer;
        }
        else if (startCountDown)
        {
            StartBtn.SetActive(true);
            notFullGameTimer -= Time.deltaTime;
            timerToStartGame = notFullGameTimer;
        }

        string tempTimer = string.Format("{0:00}", timerToStartGame);
        timerToStartDisplay.text = tempTimer;

        if (timerToStartGame <= 0f)
        {
            if (startingGame)
            {
                return;
            }
            StartGame();
        }
    }
    /// <summary>
    /// Close off room and load the game scenen.
    /// </summary>
    public void StartGame()
    {
        startingGame = true;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(gameScene);
        }
        
    }

    /// <summary>
    /// Leave the game
    /// </summary>
    public void LeaveRoom()
    {
        Debug.Log("leave room");
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(lobbyScene);
    }

    /// <summary>
    /// Enable and disable the map selector
    /// </summary>
    public void toggleMapSelector()
    {
        if (mapSelector.activeSelf)
        {
            mapSelector.SetActive(false);
        }
        else
        {
            mapSelector.SetActive(true);
        }
    }
}
