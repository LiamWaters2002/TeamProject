using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    public InputField usernameField;
    public InputField passwordField;
    public userInsert userInsert;
    public userSelect userSelect;
    private string username;
    private string password;
    public void endEdit()
    {
        username = usernameField.text;
        password = passwordField.text;
    }


    public void login()
    {
        userInsert.AddUser("0", "0", "0", "0");
        
    }

    public void PhotonNickName()
    {
        PhotonNetwork.NickName = username;
    }
}
