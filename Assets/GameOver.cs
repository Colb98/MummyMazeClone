﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    Canvas GameOverAsset;

    public void BackToMenu()
    {
        WorldManager.firstRun = true;
        SceneManager.LoadScene(0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            Instantiate(GameOverAsset);
        }
        else if(collision.gameObject.name.Contains("Mummy") || collision.gameObject.name.Contains("Scorpion"))
        {
            Debug.Log("HIT!");
            WorldManager.RemoveMummy();
        }
    }
}