using UnityEngine.SceneManagement;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    private static int level = 1;
    // The WorldManager GameObject
    [SerializeField]
    private GameObject wmGameObject;
    public void GoToNextLevel()
    {
        //int thisSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //if (thisSceneIndex < SceneManager.sceneCountInBuildSettings - 1)
        //    SceneManager.LoadScene(thisSceneIndex + 1);
        level++;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        wmGameObject.GetComponent<WorldManager>().GoToLevel(level);
    }
}
