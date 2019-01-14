using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public static bool CanContinue = true;
    public static void SaveLevel(int level)
    {
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.Save();
    }

    public void LoadLevel()
    {
        int level = PlayerPrefs.GetInt("level");
        Debug.Log(level);
        if (level < 1 || level > 19)
            CanContinue = false;

        if (!CanContinue)
            return;

        WorldManager.SetLevel(level);
        SceneManager.LoadScene("LevelTest");
    }
}
