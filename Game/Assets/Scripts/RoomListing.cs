using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomListing : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private RoomInformation roomInformation;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            RoomInformation listing = Instantiate(roomInformation, content);
            if(listing != null)
            {
                listing.SetRoomInfo(info);
            }
        }
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("name");
    }

}
