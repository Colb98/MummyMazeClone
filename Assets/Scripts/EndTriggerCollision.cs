using UnityEngine;
using UnityEngine.UI;

public class EndTriggerCollision : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    Canvas endGame;

    static Canvas c = null;

    private void OnTriggerEnter(Collider other)
    {
        // TODO: remove this and make another good endgame :/
        if (!other.gameObject.name.Contains("Player"))
            return;
        bool end = false;
        int level = WorldManager.GetLevel();
        if(level < 19)
        {
            c = Instantiate(canvas);
        }
        else
        {
            end = true;
            c = Instantiate(endGame);
        }
        c.enabled = true;
        if (end)
        {
            new WaitForSeconds(10);
            Application.Quit();
        }
    }

    public void NextLevel()
    {
        int level = WorldManager.GetLevel();
        level++;
        LoadGame.SaveLevel(level);
        c.enabled = false;
        Debug.Log("Level " + (level - 1).ToString() + " END");
        if (level < 19)
            WorldManager.GoToLevel(level);
    }
}
