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
    public Vector2 force;
    private float damage = 0.5f;
    private Vector3 projectoryPosition { get { return transform.position; } }

    public GameObject snowBallEffect;
    private bool projectileThrown;
    private GameObject thrower;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        photonView = GetComponent<PhotonView>();
    }

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
        this.force = force;
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject blood;
        if (thrower == null && other.gameObject.tag == "Player")//First collision will be the player holding the projectile
        {
            thrower = other.gameObject;
            return;
        }
        else if(thrower != other.gameObject && other.gameObject.tag == "Player")
        {
            blood = Instantiate(snowBallEffect, transform.position, transform.rotation);//display blood
            HealthBar health = other.gameObject.GetComponent<HealthBar>();
            health.takeDamage(0.25f);
            Rigidbody2D rigidBody = other.gameObject.GetComponent<Rigidbody2D>();
            rigidBody.velocity = force;
            Destroy(gameObject);
            waitForSeconds(blood);
        }

        //Ignore these collisions
        if (other.gameObject.tag == "OneWayPlatform" || thrower == other.gameObject)
        {
            return;
        }
        
        Camera camera = new Camera();
        Destroy(gameObject);
        //waitForSeconds(camera.gameObject);

    }

    IEnumerator waitForSeconds(GameObject gameobject)
    {
        
        yield return new WaitForSeconds(2.5f);
        Destroy(gameobject);
    }

}
