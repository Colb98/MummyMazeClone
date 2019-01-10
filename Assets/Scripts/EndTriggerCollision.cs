using UnityEngine;

public class EndTriggerCollision : MonoBehaviour
{
    private static int level = 1;
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    Canvas endGame;

    static Canvas c = null;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.name.Contains("Player"))
            return;
        bool end = false;
        if(level < 20)
        {
            c = Instantiate(canvas);
            level++;
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
        c.enabled = false;
        Debug.Log("Level " + (level - 1).ToString() + " END");
        if (level <= 20)
            WorldManager.GoToLevel(level);
    }

    public static void ResetLevel()
    {
        level = 1;
    }
}
