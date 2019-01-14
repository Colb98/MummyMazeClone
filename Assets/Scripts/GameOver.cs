using System.Collections;
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
        LoadGame.SaveLevel(0);
        WorldManager.ResetLevel();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            Canvas canvas = Instantiate(GameOverAsset);
            canvas.worldCamera = Camera.main;
        }
        else if(collision.gameObject.name.Contains("Mummy") || collision.gameObject.name.Contains("Scorpion"))
        {
            Debug.Log("HIT!");
            WorldManager.RemoveMummy();
        }
    }
}
