using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRotate : MonoBehaviour
{

    void Update()
    {

        transform.Rotate(0f, 0f, -10f * Time.deltaTime, Space.Self);
    }
}
