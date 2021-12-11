using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    public float speed;
    public float jumpForce;

    private float x;
    private Rigidbody2D rigidbody;

    public Transform groundCheckPoint;
    public float groundCheckRadius;
    public LayerMask whatIsGround;

    public bool isGrounded;

    private Vector3 smoothMove;

    public GameObject Shurican;
    public Transform Throwpoint;

    public PhotonView pv;

    public SpriteRenderer spriteRenderer;
    public Text playerNameText;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        if (photonView.IsMine)
        {
            playerNameText.text = PhotonNetwork.NickName;
        }
        else
        {
            playerNameText.text = pv.Owner.NickName;
        }

        
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            
            ProcessInputs();
        }
        else
        {
            smoothMovement();
        }
    }

    private void smoothMovement()
    {
        transform.position = Vector3.Lerp(transform.position, smoothMove, Time.deltaTime * 10);
    }

    private void ProcessInputs()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), 0);
        transform.position += move * speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            spriteRenderer.flipX = true;
            pv.RPC("OnDirectionChange_LEFT", RpcTarget.Others);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            spriteRenderer.flipX = false;
            pv.RPC("OnDirectionChange_RIGHT",RpcTarget.Others);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);

        if (isGrounded && (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (spriteRenderer.flipX == true)
            {
                PhotonNetwork.Instantiate(Shurican.name, Throwpoint.position, Throwpoint.rotation);//Throw shurican
            }
            else
            {
                PhotonNetwork.Instantiate(Shurican.name, Throwpoint.position, Throwpoint.rotation);//Throw shurican
            }
            
        }

    }

    [PunRPC]
    void OnDirectionChange_LEFT()
    {
        spriteRenderer.flipX = true;
    }

    [PunRPC]
    void OnDirectionChange_RIGHT()
    {
        spriteRenderer.flipX = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else if (stream.IsReading)
        {
            smoothMove = (Vector3)stream.ReceiveNext();
        }

    }
}
