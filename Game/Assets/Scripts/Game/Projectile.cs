using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    Camera camera;
    public float ballSpeed;

    private Rigidbody2D rigidBody;
    private PhotonView photonView;
    private CircleCollider2D circleCollider;
    private int launchForce = 20;
    private float damage = 0.5f;
    private Vector3 projectoryPosition { get { return transform.position; } }

    public GameObject snowBallEffect;
    private bool projectileThrown;
    private GameObject thrower;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
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
    public PhotonView getPhotonView()
    {
        return photonView;
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
        if (thrower == null && other.gameObject.tag == "Player")//First collision will be the player holding the projectile
        {
            thrower = other.gameObject;
            return;
        }
        else if(thrower != other.gameObject && other.gameObject.tag == "Player")
        {
            HealthBar health = other.gameObject.GetComponent<HealthBar>();
            health.takeDamage(0.25f);
            Destroy(gameObject);
        }

        //Ignore these collisions
        if (other.gameObject.tag == "OneWayPlatform" || thrower == other.gameObject)
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
