using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupManager : MonoBehaviourPun
{
    //Scene
    public int lobbyScene = 1;
    public int gameScene = 2;
    GameObject popup;

    private void Start()
    {
        GameObject gameCanvas = GameObject.Find("GameCanvas");
        popup = GetChildObjectByTag(gameCanvas, "Popup");
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

    //Button Click
    public void btnExitClicked()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(lobbyScene);
    }

    public void btnRematchClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.LoadLevel(gameScene);
        }
        
    }

    public void btnSpectateClicked()
    {
        popup.SetActive(false);
    }
}
