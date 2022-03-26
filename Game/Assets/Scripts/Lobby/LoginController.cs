using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    string link = "https://team25project.000webhostapp.com/userVerifyPassword.php";
    public InputField usernameField;
    public InputField passwordField;
    public userInsert dbInsert;
    public userSelect dbSelect;

    //Popup
    [SerializeField]
    public GameObject popupBox;
    [SerializeField]
    public Button exitPopupBtn;
    [SerializeField]
    public Text errorMessage;

    private string username;
    private string password;
    private bool verifiedPassword;

    private void Start()
    {
        dbInsert = GetComponent<userInsert>();
        dbSelect = GetComponent<userSelect>();
    }



    IEnumerator VerifyPassword(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        WWW verify = new WWW(link, form);
        yield return verify; //wait until users have been returned.
        string output = verify.text;
        Debug.Log(output);
        if(output.Contains("true"))
        {
            PhotonNetwork.NickName = username;
            Debug.Log("Photon Nickname Set!");
        }
        else
        {
            Debug.Log("Credentials are incorrect. Try again...");
        }
        Debug.Log("password verified:" + verifiedPassword);
    }




    public void finishEditing()
    {
        username = usernameField.text;
        password = passwordField.text;
        Debug.Log("username: " + username + " password: " + password);
    }



    public void register()
    {
        bool unique = true;
        for(int i = 0; i<dbSelect.usersData.Length-1; i++) {
            if (dbSelect.GetUserField(dbSelect.usersData[i], "username") == username)
            {
                unique = false;
                break;
            }
        }
        if (unique)
        {
            dbInsert.AddUser(username, password, "0", "0");
        }
        
    }




    public void login()
    {
        StartCoroutine(VerifyPassword(username, password));
    }
}
