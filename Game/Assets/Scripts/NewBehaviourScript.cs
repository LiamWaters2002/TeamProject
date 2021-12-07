using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NewBehaviourScript : MonoBehaviourPun, IPunObservable
{
    public float moveSpeed;
    public float jumpForce;

    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode attack;

    private Rigidbody2D theRB;

    public Transform groundCheckPoint;
    public float groundCheckRadius;
    public LayerMask whatIsGround;

    public bool isGrounded;

    public GameObject snowBall;
    public Transform Throwpoint;

    private PhotonView phoyonView;
    private Vector2 smoothMove;

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        photonView.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);
            Vector2 characterDirection = transform.localScale;

            if(Input.GetKey(left))
            {
                theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);
                characterDirection.x = -1; 
            } else if (Input.GetKey(right))
            {
                theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);
                characterDirection.x = 1;
            } else
            {
                theRB.velocity = new Vector2(0, theRB.velocity.y);
            }
            transform.localScale = characterDirection; //Change direction

            if(Input.GetKeyDown (jump)  && isGrounded)
            {
                theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
            }

            if (Input.GetKeyDown(attack))
            {
                Instantiate(snowBall, Throwpoint.position, Throwpoint.rotation);
            }
        }
        else
        {
            smoothMovement();
        }

        void smoothMovement()
        {
            transform.position = Vector2.Lerp(transform.position, smoothMove, Time.deltaTime*10);
        }
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)//Sending position when player is moving
        {
            stream.SendNext(transform.position);
        }
        else if (stream.IsReading)//Recieving position
        {
            smoothMove = (Vector2) stream.ReceiveNext();
        }
    }
}
