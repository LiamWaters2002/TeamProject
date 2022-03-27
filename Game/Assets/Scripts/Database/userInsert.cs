using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userInsert : MonoBehaviour
{
    string link = "https://team25project.000webhostapp.com/userInsert.php";

    public string inputUsername;
    public string inputPassword;
    public string inputRank;
    public string inputKills;
    public string inputDeaths;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddUser(inputUsername, inputPassword, inputKills, inputDeaths);
        }
    }

    public void AddUser(string username, string password, string kills, string deaths)
    {
        WWWForm form = new WWWForm();
        form.AddField("addUsername", username);
        form.AddField("addPassword", password);
        form.AddField("addKills", kills);
        form.AddField("addDeaths", deaths);

        WWW www = new WWW(link, form);
    }

}