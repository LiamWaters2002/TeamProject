using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userInsert : MonoBehaviour
{
    string link = "https://team25project.000webhostapp.com/userInsert.php";
    [SerializeField]
    public string inputUsername, inputPassword, inputRank, inputKills, inputDeaths;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddUser(inputUsername, inputPassword, inputRank, inputKills, inputDeaths);
        }
    }

    public void AddUser(string username, string password, string rank, string kills, string deaths)
    {
        WWWForm form = new WWWForm();
        form.AddField("addUsername", username);
        form.AddField("addPassword", password);
        form.AddField("addRank", rank);
        form.AddField("addKills", kills);
        form.AddField("addDeaths", deaths);

        WWW www = new WWW(link, form);
    }

}