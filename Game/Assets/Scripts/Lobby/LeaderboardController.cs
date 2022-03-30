using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardController : MonoBehaviour
{
    public DbSelect dbSelect;
    private string[] playersData;
    public GameObject leaderboard;

    //Fields
    public Text[] rankedNames;
    public Text[] rankedKills;
    public Text[] rankedDeaths;
    public Text[] rankedKd;

    public void Start()
    {
        dbSelect = GetComponent<DbSelect>();
    }

    private void Update()
    {
        playersData = dbSelect.GetPlayersData();
        if (leaderboard.activeSelf)
        {
            rankPlayers();
        }
        
    }

    public void rankPlayers()
    {
        Debug.Log(playersData.Length);
        for (int i = 0; i < 5; i++)
        {
            //rankedNames.Length - i
            rankedNames[i].text = dbSelect.GetPlayerStats(playersData[i], "username");
            rankedKills[i].text = dbSelect.GetPlayerStats(playersData[i], "kills");
            rankedDeaths[i].text = dbSelect.GetPlayerStats(playersData[i], "deaths");
            rankedKd[i].text = dbSelect.GetPlayerStats(playersData[i], "kd");
        }        
    }
}
