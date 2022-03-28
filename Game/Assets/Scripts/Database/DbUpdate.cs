using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DbUpdate : MonoBehaviour
{
    string link = "https://team25project.000webhostapp.com/userUpdate.php";
    public string inputUsername;
    public string inputPassword;
    public string inputKills;
    public string inputDeaths;
    public string inputKd;
    public string field;
    public string condition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateUser(inputUsername, inputPassword, inputKills, inputDeaths, inputKd, field, condition);
        }
    }

    public void UpdateUser(string username, string password, string kills, string deaths, string kd, string field, string condition)
    {
        WWWForm form = new WWWForm();
        form.AddField ("editUsername", username);
        form.AddField ("editPassword", password);
        form.AddField("editKills", kills);
        form.AddField("editDeaths", deaths);
        form.AddField("editKd", kd);

        //Field and condition
        form.AddField ("field",field);
        form.AddField("condition", condition);

        WWW www = new WWW(link, form);
        print(new WWW(link, form).ToString());
    }
}