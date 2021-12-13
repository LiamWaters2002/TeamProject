using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

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
    public int lobbyScene = 1;

    private Vector3 smoothMove;

    public GameObject Shurican;
    public Transform Throwpoint;

    public PhotonView pv;

    public SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite[] sprites;
    public ArrayList takenSprites = new ArrayList();

    public Queue<string> playerTurnOrder;
    public Text playerNameText;
    [SerializeField]
    public bool isTurn = false;
    [SerializeField]
    Button waitTurn;
    [SerializeField]
    Button endTurn;

    void Start()
    {
        playerTurnOrder = new Queue<string>();
        rigidbody = GetComponent<Rigidbody2D>();
        
        if (photonView.IsMine)
        {
            playerNameText.text = PhotonNetwork.NickName;

            //Random Sprites
            int index = randomPlayerColour();
            pv.RPC("changePlayerColour", RpcTarget.Others, index);
        }
        else
        {
            playerNameText.text = pv.Owner.NickName;
        }

        waitTurn = GameObject.Find("WaitTurnButton").GetComponent<Button>();
        endTurn = GameObject.Find("EndTurnButton").GetComponent<Button>();

        endTurn.onClick.AddListener(() => endPlayerTurn()); ; //.......................................................................................

        Debug.Log(PhotonNetwork.IsMasterClient);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("We reached here");
            //Add all players to queue.
            //foreach (Player player in PhotonNetwork.PlayerList)
            //{
            //    Debug.Log("loop");
            //    playerTurnOrder.Enqueue(player.NickName);
                
            //}
            //string whosTurn = playerTurnOrder.Dequeue();
            pv.RPC("RPCswitchTurn", RpcTarget.Others);     //change to RPCendPlayerTurn
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

        try
        {
            Vector3 pos = waitTurn.transform.position;
            if (isTurn)
            {
                waitTurn.gameObject.SetActive(false);
            }
            else
            {
                waitTurn.gameObject.SetActive(true);
            }
        }
        catch(System.NullReferenceException e)
        {
            //Object empty
        }
        
    }

    private void smoothMovement()
    {
        transform.position = Vector3.Lerp(transform.position, smoothMove, Time.deltaTime * 10);
    }

    private void ProcessInputs()
    {

        var move = new Vector3(Input.GetAxis("Horizontal"), 0);
        
        if (isTurn)
        {
            transform.position += move * speed * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                spriteRenderer.flipX = true;
                pv.RPC("OnDirectionChange_LEFT", RpcTarget.Others);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                spriteRenderer.flipX = false;
                pv.RPC("OnDirectionChange_RIGHT", RpcTarget.Others);
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(lobbyScene);
        }
            
        //Change colour of player if not all sprites are taken.
        if (Input.GetKeyDown(KeyCode.C))
        {

            int index = randomPlayerColour();

            pv.RPC("changePlayerColour", RpcTarget.Others, index);

        }    
    }

    public int randomPlayerColour()
    {
        int index = Random.Range(0, 4);
        if (takenSprites.Contains(sprites[index]))
        {
            while (takenSprites.Contains(sprites[index]))
            {
                index = Random.Range(0, 4);
            }

        }
        takenSprites.Remove(spriteRenderer.sprite);
        takenSprites.Add(sprites[index]);
        spriteRenderer.sprite = sprites[index];
        return index;
    }

    public void endPlayerTurn()
    {
        //Debug.Log("endturn");
        //playerTurnOrder.Enqueue(PhotonNetwork.LocalPlayer.NickName);
        //string whosTurn = playerTurnOrder.Dequeue();
        isTurn = false;
        pv.RPC("RPCswitchTurn", RpcTarget.Others); //switch to RPCendPlayerTurn
    }

    //[PunRPC]
    //public void RPCendPlayerTurn(Queue<string> playerTurnOrder, string whosTurn)
    //{
    //    if (PhotonNetwork.NickName == whosTurn)
    //    {
    //        isTurn = true;
    //    }
    //    else
    //    {
    //        isTurn = false;
    //    }
    //}

    
        [PunRPC]
    public void RPCswitchTurn()
    {
         isTurn = true;
    }

    [PunRPC]
    void changePlayerColour(int index)
    {
        takenSprites.Remove(spriteRenderer.sprite);
        takenSprites.Add(sprites[index]);
        spriteRenderer.sprite = sprites[index];
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
