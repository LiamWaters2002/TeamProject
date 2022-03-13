using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userInsert : MonoBehaviour
{
    string URL = "http://localhost/mydb/userInsert.php";
    [SerializeField]
    public string InputUsername, InputEmail, InputPassword, InputRank, InputCoins, InputKD;


    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddUser(InputUsername, InputEmail, InputPassword, InputRank, InputCoins, InputKD);
        }
    }



    public void AddUser(string username, string email, string password, string rank, string coins, string kd)
    {
        WWWForm form = new WWWForm();
        form.AddField("addUsername", username);
        form.AddField("addEmail", email);
        form.AddField("addPassword", password);
        form.AddField("addRank", rank);
        form.AddField("addCoins", coins);
        form.AddField("addKD", kd);

        WWW www = new WWW(URL, form);
    }

}