using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userSelect : MonoBehaviour
{
    string URL = "http://localhost/mydb/userSelect.php";
    [SerializeField]
    public string[] usersData;

    IEnumerator Start()
    {
        WWW users = new WWW (URL);
        yield return users;
        string usersDataString = users.text;
        usersData = usersDataString.Split(';');
        print (GetValueData(usersData[0], "username:"));//Change to any field
    }

    string GetValueData(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index)+index.Length);
        if (value.Contains("|"))
        {
            value = value.Remove(value.IndexOf("|"));
        }
        return value;

    }

    void Update()
    {

    }
}
