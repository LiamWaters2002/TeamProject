using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DbInsert : MonoBehaviour
{
    private string link = "https://team25project.000webhostapp.com/userInsert.php";

    public string inputUsername;
    public string inputPassword;
    public string inputRank;
    public string inputKills;
    public string inputDeaths;
    public string inputKd;

    public DbSelect dbSelect;

    public void AddUser(string username, string password, string kills, string deaths, string kd)
    {
        WWWForm form = new WWWForm();
        form.AddField("addUsername", username);
        form.AddField("addPassword", password);
        form.AddField("addKills", kills);
        form.AddField("addDeaths", deaths);
        form.AddField("addKd", kd);

        WWW www = new WWW(link, form);
    }

    public void calculateKD()
    {
        string[] playersData = dbSelect.GetPlayersData();
        int count = 0;
        for (int i = 0; i < playersData.Length; i++)
        {
            if (dbSelect.GetPlayerStats(playersData[i], "name") == "P1") //Player 1's and Players 2's names here...
            {
                count++;
            }
            if (dbSelect.GetPlayerStats(playersData[i], "name") == "P2")
            {
                count++;
            }
            if (count == 2)
            {
                break;
            }
        }


    }

}