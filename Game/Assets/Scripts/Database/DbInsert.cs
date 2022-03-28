using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DbInsert : MonoBehaviour
{
    string link = "https://team25project.000webhostapp.com/userInsert.php";

    public string inputUsername;
    public string inputPassword;
    public string inputRank;
    public string inputKills;
    public string inputDeaths;
    public string inputKd;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddUser(inputUsername, inputPassword, inputKills, inputDeaths, inputKd);
        }
    }

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

}