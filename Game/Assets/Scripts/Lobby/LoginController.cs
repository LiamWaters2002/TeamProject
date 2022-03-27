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
    public Text displayUsername;

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
            displayUsername.text = "Username: " + username;
        }
        else
        {
            Debug.Log("Credentials are incorrect. Try again...");
        }
    }




    public void finishEditing()
    {
        username = usernameField.text;
        password = passwordField.text;
    }



    public void register()
    {
        bool unique = true;
        for(int i = 0; i<dbSelect.usersData.Length-1; i++) {
            if (dbSelect.GetUserField(dbSelect.usersData[i], "username") == username)
            {
                unique = false;
                errorMessage.text = "Username is not unique, try again.";
                popupBox.SetActive(true);
                break;
            }
        }
        if (unique)
        {
            dbInsert.AddUser(username, password, "0", "0");
            login();
        }
        
    }




    public void login()
    {
        StartCoroutine(VerifyPassword(username, password));
    }
}
