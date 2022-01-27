using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private PlatformEffector2D platform;

    void Start()
    {
        platform = GetComponent<PlatformEffector2D>();
    }

    public void flipCollision()
    {
        
        if (platform.rotationalOffset == 0)
        {
            platform.rotationalOffset = 180f;
        }
        else
        {
            platform.rotationalOffset = 0;
        }
        
    }
}
