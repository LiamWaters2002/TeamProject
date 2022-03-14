using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject background;

    void Start()
    {
        string backgroundName = background.GetComponent<SpriteRenderer>().sprite.name;
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("OneWayPlatform");
        foreach (GameObject platform in platforms)
        {
            if(backgroundName == "Dojo")
            {
                platform.GetComponent<SpriteRenderer>().color = Color.black;
            }
            else
            {
                platform.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
