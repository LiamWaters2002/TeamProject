using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomInformation : MonoBehaviour
{
    [SerializeField]
    private Text roomName;

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        roomName.text = roomInfo.MaxPlayers + "," + roomInfo.Name;
    }
}
