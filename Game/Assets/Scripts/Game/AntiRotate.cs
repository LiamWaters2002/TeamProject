using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRotate : MonoBehaviour
{
    /// <summary>
    /// Used to rotate the camera of the projectile the opposite direction.
    /// So that it looks as if the camera isnt rotating at all.
    /// </summary>
    void Update()
    {
        transform.Rotate(0f, 0f, -10f * Time.deltaTime, Space.Self);
    }
}
