using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private Scene _actualScene;
    private void Awake()
    {
        _actualScene = SceneManager.GetActiveScene();
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(_actualScene.buildIndex + 1);
        }
    }
}
