using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField]
    int trajectoryDots;
    [SerializeField]
    GameObject dotsParent;
    [SerializeField]
    GameObject dotPrefab;
    [SerializeField]
    float dotSpacing;

    Vector2 dotPosition;
    float timeStamp;
    Transform[] dotList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void prepareDots()
    {
        dotList = new Transform[trajectoryDots];

        for (int i = 0; i < trajectoryDots; i++)
        {
            dotList[i] = Instantiate(dotPrefab, null).transform;
            dotList[i].parent = dotsParent.transform;
        }
    }

    public void updateDots(Vector3 projPosition, Vector2 force)
    {
        timeStamp = dotSpacing;
        for(int i = 0; i < trajectoryDots; i++)
        {
            dotPosition.x = projPosition.x + force.x * timeStamp;
            dotPosition.x = projPosition.y + force.y * timeStamp - (Physics2D.gravity.magnitude*timeStamp*timeStamp)/2f;

            dotList[i].position = dotPosition;
            timeStamp += dotSpacing;
        }
    }

    public void display()
    {
        dotsParent.SetActive(true);
    }

    public void hide()
    {
        dotsParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
