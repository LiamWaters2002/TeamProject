using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userSelect : MonoBehaviour
{
    string link = "https://team25project.000webhostapp.com/userSelect.php";
    [SerializeField]
    public string[] usersData;

    IEnumerator Start()
    {
        WWW users = new WWW (link);
        yield return users; //wait until users have been returned.
        string output = users.text;
        usersData = output.Split(';');
        print( GetUserField(usersData[0], "rank") );//Change to any field
    }

    string GetUserField(string userData, string field)
    {
        field = field + ":";
        string value = userData.Substring(userData.IndexOf(field)+field.Length);//Get everything after "field:"
        if (value.Contains("|"))
        {
            value = value.Remove(value.IndexOf("|"));//Remove everything after |
        }
        return value;

    }
}
