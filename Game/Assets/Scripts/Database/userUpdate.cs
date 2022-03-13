using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userUpdate : MonoBehaviour
{
    string URL = "http://localhost/mydb/userUpdate.php";
    public string InputUsername, InputEmail, InputPassword, InputRank, InputCoins, InputKD, WhereField, WhereCondition;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateUser(InputUsername, InputEmail, InputPassword, InputRank, InputCoins, InputKD, WhereField, WhereCondition);
        }
    }

    public void UpdateUser(string username, string email, string password, string rank, string coins, string KD, string wF, string wC)
    {
        WWWForm form = new WWWForm();
        form.AddField ("editUsername", username);
        form.AddField ("editEmail", email);
        form.AddField ("editPassword", password);
        form.AddField("editRank", rank);
        form.AddField("editCoins", coins);
        form.AddField("editKD", KD);

        form.AddField ("whereField",wF);
        form.AddField("whereCondition", wC);

        WWW www = new WWW(URL, form);
    }
}