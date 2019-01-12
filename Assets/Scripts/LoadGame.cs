using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
            level = 1;

        WorldManager.SetLevel(level);
        SceneManager.LoadScene("LevelTest");
    }
}
