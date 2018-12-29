using UnityEngine.SceneManagement;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    public void GoToNextLevel()
    {
        int thisSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (thisSceneIndex < SceneManager.sceneCountInBuildSettings - 1)
            SceneManager.LoadScene(thisSceneIndex + 1);
    }
}
