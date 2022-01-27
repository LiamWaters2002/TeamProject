using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerNetworking : MonoBehaviour
{

    public MonoBehaviour[] scriptsToIgnore;

    private PhotonView photonView;
    private Rigidbody2D rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        rigidbody = GetComponent<Rigidbody2D>();
        if (!photonView.IsMine)
        {

            foreach (var script in scriptsToIgnore)
            {
                script.enabled = false;
            }
        }
    }
}
