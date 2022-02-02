using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    //Player Movement
    public float speed;
    public float jumpForce;
    private Rigidbody2D rigidbody;

    //Ground Check
    public Transform groundCheckPoint;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    public bool isGrounded;
    private GameObject currentOneWayPlatform;
    private BoxCollider2D playerCollider;

    //Scene
    public int lobbyScene = 1;
    [SerializeField]
    private GameObject sceneCamera;
    [SerializeField]
    public GameObject playerCamera;

    //Make other player movement smoother
    private Vector3 otherMove;

    //Projectiles
    private int selectedProjectile = 0;
    public GameObject[] projectile;
    public Projectile projectileInstance;
    public Transform Throwpoint;
    float pushForce = 4f;

    //Aiming
    public Trajectory trajectory;
    Vector2 startPoint; //start position of mouse click
    Vector2 endPoint; //end position of mouse click
    Vector2 direction;
    Vector2 force;
    float distance; //distance from start to end
    bool aiming;
    Camera camera;

    //Photon View
    public PhotonView pv;

    //Sprites
    public SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite[] sprites;
    public ArrayList takenSprites = new ArrayList();

    //Turn-based Mechanics
    Timer timer;
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
        camera = Camera.main;

        playerTurnOrder = new Queue<string>();
        rigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        trajectory = GameObject.FindObjectOfType<Trajectory>();

        if (photonView.IsMine)
        {
            //Display player's username
            playerNameText.text = PhotonNetwork.NickName;

            //Random Sprites
            int index = randomPlayerColour();
            pv.RPC("changePlayerColour", RpcTarget.Others, index);

            //Camera - lines below are to be used later on...
            sceneCamera = GameObject.Find("SceneCamera");
            sceneCamera.SetActive(false);
            playerCamera.SetActive(true);
            //playerCamera.SetActive(false);
        }
        else
        {
            //Display other usernames
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
            //This line above switches it to others...
        }

    }

    void Update()
    {
        //if(timer.nextTimeMode)
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
        catch (System.NullReferenceException e)
        {
            //Object empty
        }

    }

    private void smoothMovement()
    {
        transform.position = Vector3.Lerp(transform.position, otherMove, Time.deltaTime * 10);
    }

    private void ProcessInputs()
    {

        var move = new Vector3(Input.GetAxis("Horizontal"), 0);

        if (isTurn)
        {
            if (!aiming)
            {
                transform.position += move * speed * Time.deltaTime;
            }

            //Holding the aim button
            if (Input.GetMouseButtonDown(1))
            {
                //Before start aiming, set start point of aim
                //if(aiming == false){}
                startPoint = camera.ScreenToWorldPoint(Input.mousePosition);
                aiming = true;
                trajectory.display();
                projectileInstance = PhotonNetwork.Instantiate(projectile[selectedProjectile].name, Throwpoint.position, Throwpoint.rotation).GetComponent<Projectile>();
                
            }

            if (aiming)
            {
                endPoint = camera.ScreenToWorldPoint(Input.mousePosition);
                distance = Vector2.Distance(startPoint, endPoint);
                direction = (startPoint - endPoint).normalized;
                force = direction * distance * pushForce;
                //trajectory.updateDots(shuricanInstance.getPosition(), force);
                //Debug draw line
                Debug.DrawLine(startPoint, endPoint);

                if (startPoint.x < endPoint.x)
                {
                    spriteRenderer.flipX = true;
                    pv.RPC("OnDirectionChange_LEFT", RpcTarget.Others);
                }
                else
                {
                    spriteRenderer.flipX = false;
                    pv.RPC("OnDirectionChange_RIGHT", RpcTarget.Others);
                }
            }

            //Throw
            if (Input.GetMouseButtonDown(0) && aiming)
            {
                trajectory.hide();
                aiming = false;
                Vector3 vector3Force = force;
                pv.RPC("ThrowProjectile", RpcTarget.All, vector3Force);
            }

            //Stop aiming
            if (Input.GetMouseButtonUp(1) && aiming)
            {
                aiming = false;
                PhotonNetwork.Instantiate(projectile[0].name, Throwpoint.position, Throwpoint.rotation);
            }

            if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && !aiming)
            {
                spriteRenderer.flipX = true;
                pv.RPC("OnDirectionChange_LEFT", RpcTarget.Others);
            }

            if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && !aiming)
            {
                spriteRenderer.flipX = false;
                pv.RPC("OnDirectionChange_RIGHT", RpcTarget.Others);
            }

            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);

            if (isGrounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !aiming)
            {
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                if (currentOneWayPlatform != null)
                {
                    StartCoroutine(ignoreCollision());
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (sceneCamera.activeSelf == false)
            {
                sceneCamera.SetActive(true);
                playerCamera.SetActive(false);
            }
            else
            {
                sceneCamera.SetActive(false);
                playerCamera.SetActive(true);
            }

        }

        for (int i = 1; i < projectile.Length + 1; i++)
        {
            if (Input.GetKeyDown(""+i))
            {
                selectedProjectile = i;
            }
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

    [PunRPC]
    void ThrowProjectile(Vector3 force)
    {
        Vector2 vector2Force = force;
        Projectile projectileInstance = GameObject.FindObjectOfType<Projectile>();
        projectileInstance.projectileForce(vector2Force);
        projectileInstance.isKinematic();
        projectileInstance.throwProjectile();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else if (stream.IsReading)
        {
            otherMove = (Vector3)stream.ReceiveNext();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        currentOneWayPlatform = null;
    }

    private IEnumerator ignoreCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        if (platformCollider == null)
        {
            Debug.Log("platform null");
        }
        if (playerCollider == null)
        {
            Debug.Log("player null");
        }
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.50f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
