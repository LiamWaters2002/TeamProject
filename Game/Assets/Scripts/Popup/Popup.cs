using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Popup : MonoBehaviourPun, IPunObservable
{
    //In-game popups
    private GameObject gameCanvas;
    private GameObject popupBox;
    private GameObject btnExit;
    private GameObject btnRematch;
    private GameObject btnSpectate;
    private Text textMessage;
    private Text textRematch;

    public void Start()
    {
        gameCanvas = GameObject.Find("GameCanvas");
        popupBox = GetChildObjectByTag(gameCanvas, "Popup");
        textMessage = GetChildObjectByTag(popupBox, "popupMessage").GetComponent<Text>();
        textRematch = GetChildObjectByTag(popupBox, "popupRematchText").GetComponent<Text>();
        btnExit = GetChildObjectByTag(popupBox, "btnExit");
        btnRematch = GetChildObjectByTag(popupBox, "btnRematch");
        ////btnSpectate = GetChildObjectByTag(popupBox, "//btnSpectate");
        hidePopup();
    }

    private GameObject GetChildObjectByTag(GameObject parent, string tag)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).tag == tag)
            {
                return parent.transform.GetChild(i).gameObject;
            }
        }
        return null;
    }

    public void hidePopup()
    {
        popupBox.SetActive(false);
        btnExit.SetActive(false);
        btnRematch.SetActive(false);
        ////btnSpectate.SetActive(false);
        textRematch.gameObject.SetActive(false);
    }

    public void Update()
    {

        if (photonView.IsMine & Input.GetKeyDown(KeyCode.Escape))
        {
            if (popupBox.activeInHierarchy == true)
            {
                hidePopup();
            }
            else if (popupBox.activeInHierarchy == false)
            {
                escapePopup();
            }
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (photonView.IsMine)
        {
            if (photonView.gameObject.GetComponent<HealthBar>().healthBar.fillAmount == 0)
            {
                deathPopup();
            }

            if (players.Length == 1)
            {
                if (players[0] == photonView.gameObject)
                {
                    winPopup();
                }

            }
        }

        if (players.Length == 1)
        {
            //btnSpectate.gameObject.SetActive(false);
            if (PhotonNetwork.IsMasterClient)
            {
                btnRematch.gameObject.SetActive(true);
            }
            else
            {
                textRematch.gameObject.SetActive(true);
            }
            
        }
    }

    private IEnumerator waitTime()
     {
        yield return new WaitForSeconds(6f);
     }


    public void escapePopup()
    {
        textMessage.text = "Main Menu";
        popupBox.SetActive(true);
        btnExit.SetActive(true);
    }

    public void deathPopup()
    {
        textMessage.text = "You died!";
        popupBox.SetActive(true);
        btnExit.gameObject.SetActive(true);
        //btnSpectate.gameObject.SetActive(true);
    }

    public void winPopup()
    {
        textMessage.text = "You Won!";
        popupBox.SetActive(true);
        btnExit.gameObject.SetActive(true);
    }
 
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
