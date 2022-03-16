using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userDelete : MonoBehaviour
{
    string link = "https://team25project.000webhostapp.com/userDelete.php";
    public string field, condition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DelUser(field, condition);
        }
    }

    public void DelUser(string field, string condition)
    {
        WWWForm form = new WWWForm();
        form.AddField("field", field);
        form.AddField("condition", condition);
        WWW www = new WWW(link, form);
    }
}
