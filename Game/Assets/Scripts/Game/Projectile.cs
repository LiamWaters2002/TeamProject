using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    Camera camera;
    public float ballSpeed;

    private Rigidbody2D rigidBody;
    private CircleCollider2D circleCollider;
    private int launchForce = 20;
    private Vector3 projectoryPosition { get { return transform.position; } }

    public GameObject snowBallEffect;
    private bool projectileThrown;
    private GameObject playerObject;

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
            camera.transform.Rotate(0f, 0f, -100f * Time.deltaTime, Space.Self);//Counter rotation of projectile
            transform.Rotate(0f, 0f, 100f * Time.deltaTime, Space.Self);//Rotate projectile
            rigidBody.AddForce(transform.forward * launchForce);
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
        if (playerObject == null)//First collision will be the player holding the projectile
        {
            playerObject = other.gameObject;
            return;
        }

        //Ignore these collisions
        if (other.gameObject.tag == "OneWayPlatform" || playerObject == other.gameObject)
        {
            return;
        }
        GameObject blood = Instantiate(snowBallEffect, transform.position, transform.rotation);
        
        Camera camera = new Camera();
        Destroy(gameObject);
        waitForSeconds(blood);
        waitForSeconds(camera.gameObject);

    }

    IEnumerator waitForSeconds(GameObject gameobject)
    {
        
        yield return new WaitForSeconds(2.5f);
        Destroy(gameobject);
    }

}
