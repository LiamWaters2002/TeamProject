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
    private GameObject Start;
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

    private void Awake()
    {
        roomName.text = "Room: " + PhotonNetwork.CurrentRoom.Name;

        view = GetComponent<PhotonView>();
        fullGameTimer = maxFullGameWaitTime;
        notFullGameTimer = maxWaitTime;
        timerToStartGame = maxWaitTime;

        PlayerCountUpdate();
    }

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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerCountUpdate();
        if (PhotonNetwork.IsMasterClient)
        {
            view.RPC("RPC_Timer", RpcTarget.Others, startCountDown);
        }
    }

    [PunRPC]
    private void RPC_SyncTimer(float timeIn)
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

    void ResetTimer()
    {
        timerToStartGame = maxWaitTime;
        notFullGameTimer = maxWaitTime;
        fullGameTimer = maxFullGameWaitTime;
    }


    void WaitingForMorePlayers()
    {
        if(playerCount <= 1)
        {
            Start.SetActive(false);
            ResetTimer();
        }

        if (gameReady)
        {
            Start.SetActive(true);
            fullGameTimer -= Time.deltaTime;
            timerToStartGame = fullGameTimer;
        }
        else if (startCountDown)
        {
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
    
    public void StartGame()
    {
        startingGame = true;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(gameScene);
        }
        
    }

    public void DelayCancel()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(lobbyScene);
    }

}
