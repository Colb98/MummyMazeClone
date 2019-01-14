using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionControl : MonoBehaviour
{
    public void Confirm()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void SetLevel(int level)
    {
        WorldManager.SetLevel(level);
        LoadGame.CanContinue = false;
    }

    public void SetLevel(string levelString)
    {
        int result;

        if (int.TryParse(levelString, out result))
        {
            if (result >= 1 && result <= 19)
                SetLevel(result);
            else
                WorldManager.SetLevel(1);
        }
        else
            WorldManager.SetLevel(1);
    }

    public void SetVolume(float volume)
    {
        volume = volume / 100f;
        AudioListener.volume = volume;
    }

    public void MusicOn(bool value)
    {
        AudioListener.pause = !value;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}