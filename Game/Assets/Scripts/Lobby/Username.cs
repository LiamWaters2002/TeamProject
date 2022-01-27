using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Username : MonoBehaviour
{
    public InputField usernameField;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Username"))
        {
            string username = PlayerPrefs.GetString("Username");
            usernameField.text = username;
        }
        else
        {
            return;
        }
    }

    public void PhotonNickName()
    {
        string PhotonNickName = usernameField.text;
        PhotonNetwork.NickName = PhotonNickName;
        PlayerPrefs.SetString("Username", PhotonNickName);
    }
}
