using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;


    public int P1Life;
    public int P2Life;

    public GameObject gameOver;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if(P1Life <= 0)
        //{
        //    player1.SetActive(false);
        //    gameOver.SetActive(true);
        //}

        //if(P2Life <= 0)
        //{
        //    //player2.SetActive(false);
        //    gameOver.SetActive(true);
        //}
    }

    public void HurtP1()
    {
        P1Life -= 1;
    }

    public void HurtP2()
    {
        P2Life -= 1;
    }





}
