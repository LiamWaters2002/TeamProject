using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnyKey : MonoBehaviour
{
    private int lobbyScene = 1;
    [SerializeField]
    GameObject[] canvas;
    // Update is called once per frame
    void Start()
    {
        //var number = Random.Range(0, 2);
        //canvas[number].SetActive(false);
    }

    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(lobbyScene);
        }
    }
}
