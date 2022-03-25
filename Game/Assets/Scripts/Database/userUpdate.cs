using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userUpdate : MonoBehaviour
{
    string link = "https://team25project.000webhostapp.com/userUpdate.php";
    public string inputUsername, inputPassword, inputKills, inputDeaths, field, condition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateUser(inputUsername, inputPassword, inputKills, inputDeaths, field, condition);
        }
    }

    public void UpdateUser(string username, string password, string kills, string deaths, string field, string condition)
    {
        WWWForm form = new WWWForm();
        form.AddField ("editUsername", username);
        form.AddField ("editPassword", password);
        form.AddField("editKills", kills);
        form.AddField("editDeaths", deaths);

        //Field and condition
        form.AddField ("field",field);
        form.AddField("condition", condition);

        WWW www = new WWW(link, form);
        print(new WWW(link, form).ToString());
    }
}