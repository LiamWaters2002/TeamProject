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
        if (leaderboard.activeSelf)
        {
            playersData = dbSelect.GetPlayersData();
            rankPlayers();
        }
        
    }

    public void rankPlayers()
    {
        for (int i = 0; i < playersData.Length-1; i++)
        {
            rankedNames[playersData.Length - 2 - i].text = dbSelect.GetPlayerStats(playersData[i], "username");
            rankedKills[playersData.Length - 2 - i].text = dbSelect.GetPlayerStats(playersData[i], "kills");
            rankedDeaths[playersData.Length - 2 - i].text = dbSelect.GetPlayerStats(playersData[i], "deaths");
            rankedKd[playersData.Length - 2 - i].text = dbSelect.GetPlayerStats(playersData[i], "kd");
        }        
    }
}
