using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DbSelect : MonoBehaviour
{
    private string link = "https://team25project.000webhostapp.com/userSelect.php";
    public string[] playersData;

    /// <summary>
    /// Get output of data - which is a concatenated string of user data.
    /// split it into each seperate user
    /// </summary>
    IEnumerator Start()
    {
        WWW users = new WWW(link);
        yield return users; //wait until users have been returned.
        string output = users.text;
        playersData = output.Split(';');
    }
    /// <summary>
    /// Use index of the array playersData to get data on a specific user and a specific field.
    /// </summary>
    /// <param name="playerData"></param>
    /// <param name="field"></param>
    /// <returns></returns>
    public string GetPlayerStats(string playerData, string field)
    {
        field = field + ":";
        string value = playerData.Substring(playerData.IndexOf(field) + field.Length);//Get everything after "field:"
        if (value.Contains("|"))
        {
            value = value.Remove(value.IndexOf("|"));//Remove everything after |
        }
        return value;
    }

    public string[] GetPlayersData()
    {
        return playersData;
    }
}

