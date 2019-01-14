using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStartScene : MonoBehaviour
{
    public void StartScene()
    {
        SceneManager.LoadScene("LevelTest");
        //EventSystem.current.SetSelectedGameObject(null);
    }

    public void OptionScene()
    {
        SceneManager.LoadScene("OptionScene");
    }
}
