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
    private GameObject[] oneWayPlatforms;
    private GameObject currentOneWayPlatform;
    private BoxCollider2D playerCollider;

    //Fall Damage
    private int playerPhotonID;
    private float playerFallVelY;

    //Popup
    public Popup popup;

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
        timer = GameObject.Find("TimeManager").GetComponent<Timer>();
        
        oneWayPlatforms = GameObject.FindGameObjectsWithTag("OneWayPlatform");

        if (photonView.IsMine)
        {
            //Display player's username
            playerNameText.text = PhotonNetwork.NickName;

            //Random Sprites
            int index = RandomPlayerColour();
            pv.RPC("changePlayerColour", RpcTarget.AllBuffered, index);

            //Camera - lines below are to be used later on...
            sceneCamera = GameObject.Find("SceneCamera");
            sceneCamera.SetActive(false);
            playerCamera.SetActive(true);
        }
        else
        {
            //Display other usernames
            playerNameText.text = pv.Owner.NickName;
        }

        waitTurn = GameObject.Find("WaitTurnButton").GetComponent<Button>();
        endTurn = GameObject.Find("EndTurnButton").GetComponent<Button>();

        endTurn.onClick.AddListener(() => endPlayerTurn()); ; //.......................................................................................

        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC("RPCgivePlayerTurn", RpcTarget.Others);//Make this random
        }

    }

    void Update()
    {
        //if(timer.nextTimeMode)
        if (photonView.IsMine)
        {
            HealthBar health = PhotonView.Find(pv.ViewID).gameObject.GetComponent<HealthBar>();
            if (health.healthBar.fillAmount == 0)
            {
                sceneCamera.SetActive(true);
                playerCamera.SetActive(false);

                StartCoroutine(zeroHealth()); //Player Death

                //increment value by 1.
                //Calculate ratio.
            }

            //if (timer.getCurrentTimeMode() == "getReady" && !timer.endedTurn && isTurn)
            //{
                //timer.endedTurn = true;
                //endPlayerTurn();
            //}

            if (isTurn)
            {
                waitTurn.gameObject.SetActive(false);
            }
            else
            {
                waitTurn.gameObject.SetActive(true);
            }

            ProcessInputs();
        }
        else
        {
            smoothMovement();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject[] namesArray = GameObject.FindGameObjectsWithTag("PlayerName");
            foreach (GameObject name in namesArray)
            {
                if (sceneCamera.activeSelf == true)
                {
                    name.GetComponent<Text>().fontSize = 3;
                }
                else
                {
                    name.GetComponent<Text>().fontSize = 1;
                }
                    
            }
            
            

        }
    }

    private void smoothMovement()
    {
        transform.position = Vector3.Lerp(transform.position, otherMove, Time.deltaTime * 10);
    }

    private void ProcessInputs()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);
        var move = new Vector3(Input.GetAxis("Horizontal"), 0);

        if (isTurn && timer.canMove)
        {
            if (!aiming)
            {
                transform.position += move * speed * Time.deltaTime;
            }

            //Holding the aim button.
            if (Input.GetMouseButtonDown(1) && timer.getTime() > 1)
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
                pv.RPC("ThrowProjectile", RpcTarget.All, force);
            }

            //Stop aiming
            if (Input.GetMouseButtonUp(1) && aiming || timer.getTime() < 1 &&  aiming)
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

            if (isGrounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !aiming)
            {
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (currentOneWayPlatform != null)
                {
                    StartCoroutine(ignoreCollision());
                }
            }

        }

        //Change colour of player if not all sprites are taken.
        if (Input.GetKeyDown(KeyCode.C))
        {

            int index = RandomPlayerColour();

            pv.RPC("changePlayerColour", RpcTarget.AllBuffered, index);

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


        if (playerFallVelY > rigidbody.velocity.y)
        {
            playerFallVelY = rigidbody.velocity.y;
        }

        for (int i = 1; i < projectile.Length + 1; i++)
        {
            if (Input.GetKeyDown(""+i))
            {
                selectedProjectile = i;
            }
        }
    }

    void FixedUpdate()
    {
        
        if (photonView.IsMine && isGrounded && playerFallVelY <= -30) //Fall Damage for your player
        {
            pv.RPC("RPCdamageInfo", RpcTarget.AllBuffered, pv.ViewID, playerFallVelY);
        }
        if(playerPhotonID != 0 && playerFallVelY != 0) //Fall Damage for another player
        {
            HealthBar health = PhotonView.Find(playerPhotonID).gameObject.GetComponent<HealthBar>();
            if (playerFallVelY <= -70)
            {
                health.takeDamage(1.0f);
            }
            else if (playerFallVelY <= -60)
            {
                health.takeDamage(0.8f);
            }
            else if (playerFallVelY <= -50)
            {
                health.takeDamage(0.4f);
            }
            else if (playerFallVelY <= -40)
            {
                health.takeDamage(0.2f);
            }
            else if (playerFallVelY <= -30)
            {
                health.takeDamage(0.1f);
            }
            playerFallVelY = 0;
            playerPhotonID = 0;
        }
        
        
    }

    public void endPlayerTurn()
    {
        isTurn = false;
        pv.RPC("RPCgivePlayerTurn", RpcTarget.Others); //switch to RPCendPlayerTurn
    }

    public int RandomPlayerColour()
    {
        int index = Random.Range(0, 4);
        while (takenSprites.Contains(sprites[index]))
        {
            index = Random.Range(0, 4);
        }
       // takenSprites.Remove(spriteRenderer.sprite);
      //  takenSprites.Add(sprites[index]);
       // spriteRenderer.sprite = sprites[index];
        return index;
    }

    [PunRPC]
    public void RPCendPlayerTurn()
    {
        isTurn = false;
    }

    [PunRPC]
    public void RPCgivePlayerTurn()
    {
        isTurn = true;
    }

    [PunRPC]
    public void RPCdamageInfo(int photonViewID, float fallDamageVelY)
    {
        playerPhotonID = photonViewID;
        playerFallVelY = fallDamageVelY;
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
    void ThrowProjectile(Vector2 force)
    {
        //Vector2 vector2Force = force;
        Projectile projectileInstance = GameObject.FindObjectOfType<Projectile>();
        projectileInstance.projectileForce(force);
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
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(playerCollider, collision.gameObject.GetComponent<BoxCollider2D>());
        }

        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        currentOneWayPlatform = null;
    }

    private IEnumerator zeroHealth()
    {
        yield return new WaitForSeconds(3f);
        pv.RPC("RPCplayerDeath", RpcTarget.All, pv.ViewID);
    }

        private IEnumerator ignoreCollision()
    {
        int photonViewID = pv.ViewID;
        pv.RPC("RPCdisableCollision", RpcTarget.All, photonViewID);
        yield return new WaitForSeconds(0.40f);
        pv.RPC("RPCenableCollision", RpcTarget.All, photonViewID);
    }

    [PunRPC]
    void RPCdisableCollision(int photonViewID)
    {
        foreach (GameObject platform in oneWayPlatforms)
        {
            Physics2D.IgnoreCollision(playerCollider, platform.GetComponent<Collider2D>());
        }
        
    }

    [PunRPC]
    void RPCenableCollision(int photonViewID)
    {
        foreach (GameObject platform in oneWayPlatforms)
        {
            Physics2D.IgnoreCollision(playerCollider, platform.GetComponent<Collider2D>(), false);
        }
    }

    [PunRPC]

    void RPCplayerDeath(int photonID)
    {
        PhotonView.Find(photonID).gameObject.SetActive(false);
    }
}
