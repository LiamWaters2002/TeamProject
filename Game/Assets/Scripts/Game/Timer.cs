using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviourPun
{
    private float turnTime;
    private float getReadyTime;
    private float projectileImpactTime;
    private static float time;
    private static bool paused;
    private static string nextTimeMode;
    private static string currentTimeMode;
    public bool endedTurn;
    public bool canMove;
    private PhotonView pv;

    [SerializeField]
    public Text timer;
    // Start is called before the first frame update

    public Timer()
    {
        turnTime = 20;
        getReadyTime = 5;
        projectileImpactTime = 5;
        
    }
    
    public Timer(float turnTime, float getReadyTime, float projectileImpactTime)
    {
        this.turnTime = turnTime;
        this.getReadyTime = getReadyTime;
        this.projectileImpactTime = projectileImpactTime;
    }
    void Start()
    {
        time = getReadyTime;
        nextTimeMode = "turn";
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                timer.text = Mathf.RoundToInt(time).ToString();
            }
            else
            {
                canMove = false;
                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("next time mode: " + nextTimeMode);
                    pv.RPC("RPCsetTimeMode", RpcTarget.AllBuffered, nextTimeMode);
                }
            }
        }
        
    }
    

    public void setTimeMode(string mode)
    {
        nextTimeMode = mode;
    }

    public string getNextTimeMode()
    {
        return nextTimeMode;
    }

    public string getCurrentTimeMode()
    {
        return currentTimeMode;
    }

    public float getTime()
    {
        return time;
    }
    

    /// <summary>
    /// Time for camera to stay once the projectile has hit an object
    /// </summary>
    public void impactTimer()//Time
    {
        time = projectileImpactTime;
        timer.text = Mathf.RoundToInt(time).ToString();
    }

    public void pauseTimer()
    {
        paused = true;
    }

    public void unpauseTimer()
    {
        paused = false;
    }

    [PunRPC]
    public void RPCsetTimeMode(string nextTimeMode)
    {
        SetTimeMode(nextTimeMode);
    }
    /// <summary>
    /// Set variables for each game states the current timeMode and next timeMode allocated for these states.
    /// </summary>
    /// <param name="setTimeMode">string which specifies what game state is to be set to.</param>
    public void SetTimeMode(string setTimeMode)
    {
        if (setTimeMode == "turn")//Time for player to make their moves.
        {
            endedTurn = false;
            canMove = true;
            currentTimeMode = "turn";
            time = turnTime;
            nextTimeMode = "getReady";
            
        }
        else if (setTimeMode == "getReady")//Time give to prepare player for their turn.
        {
            currentTimeMode = "getReady";
            time = getReadyTime;
            nextTimeMode = "turn";
        }
        timer.text = Mathf.RoundToInt(time).ToString();
    }
}
