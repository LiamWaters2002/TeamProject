using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shurican : MonoBehaviour
{

    public float ballSpeed;

    private Rigidbody2D rigidBody;
    private CircleCollider2D circleCollider;
    private int launchForce = 20;
    private Vector3 projectoryPosition { get { return transform.position; } }

    public GameObject snowBallEffect;
    private bool projectileThrown;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (projectileThrown)
        {
            rigidBody.AddForce(transform.right * launchForce);
        }
        
    }

    public Vector3 getPosition()
    {
        return projectoryPosition;
    }

    public void throwProjectile()
    {
        projectileThrown = true;
    }

    public void trackMovement() { 
    }

    public void destroy()
    {
        Destroy(gameObject);
    }

    public void projectileForce(Vector2 force)
    {
        rigidBody.AddForce(force, ForceMode2D.Impulse);
    }

    public void isKinematic()
    {
        rigidBody.isKinematic = false;
    }

    public void isntKinematic()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = 0f;
        rigidBody.isKinematic = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(snowBallEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }

}
