using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;

    private float x;
    private Rigidbody2D rigidbody;

    public Transform groundCheckPoint;
    public float groundCheckRadius;
    public LayerMask whatIsGround;

    public bool isGrounded;

    public GameObject Shurican;
    public Transform Throwpoint;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        x = Input.GetAxis("Horizontal");//left = -1, nothing = 0, right = 1

        transform.position += (Vector3) new Vector2(x * speed * Time.deltaTime, 0);

        Vector2 characterDirection = transform.localScale;
        if (x > 0)
        {
            characterDirection.x = 1;
        }
        else if(x < 0)
        {
            characterDirection.x = -1;
        }
        transform.localScale = characterDirection; //Change direction
                                                   
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);

        if (isGrounded && (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) ))
        {
            rigidbody.AddForce(transform.up * jumpForce,ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            PhotonNetwork.Instantiate(Shurican.name, Throwpoint.position, Throwpoint.rotation);//Throw shurican
        }


    }
}
