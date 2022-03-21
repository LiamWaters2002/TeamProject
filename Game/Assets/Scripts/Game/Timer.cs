using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float turnTime;
    private float getReadyTime;
    private float projectileImpactTime;
    private static float time;
    private static bool paused;
    private static string nextTimeMode;
    private bool endTurn;
    public bool canMove;

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

    public void setTimeMode(string mode)
    {
        nextTimeMode = mode;
    }

    public string getTimeMode()
    {
        return nextTimeMode;
    }

    public float getTime()
    {
        return time;
    }
    void Start()
    {
        time = getReadyTime;
        nextTimeMode = "turn";
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
                resetTimer();
            }
        }
        
    }

    public void resetTimer()
    {
        if (nextTimeMode == "turn")//Time for player to make their moves.
        {
            time = turnTime;
            nextTimeMode = "getReady";
            canMove = true;
        }
        else if (nextTimeMode == "getReady")//Time give to prepare player for their turn.
        {
            time = getReadyTime;
            nextTimeMode = "turn";
            
        }
        timer.text = Mathf.RoundToInt(time).ToString();
    }

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
}
