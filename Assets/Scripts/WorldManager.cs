using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    public static GameObject player;
    public static Vector2Int playerPos;
    public static List<WhiteMummy> whiteMummies = new List<WhiteMummy>();
    public static bool movingSomething = false;
    public static int[,,] mapData;
    public static GameObject endGameTrigger;

    public void Update()
    {
        if (movingSomething)
            return;

        if (Input.GetAxis("Vertical") > 0 && playerCanMoveForward())
        {
            player.GetComponent<MovementComponent>().MoveForward();
            movingSomething = true;
        }
        else if (Input.GetAxis("Vertical") < 0 && playerCanMoveBackward())
        {
            player.GetComponent<MovementComponent>().MoveBackward();
            movingSomething = true;
        }
        else if (Input.GetAxis("Horizontal") > 0 && playerCanMoveRight())
        {
            player.GetComponent<MovementComponent>().MoveRight();
            movingSomething = true;
        }
        else if (Input.GetAxis("Horizontal") < 0 && playerCanMoveLeft())
        {
            player.GetComponent<MovementComponent>().MoveLeft();
            movingSomething = true;
        }
        // After player moved, the mummy move
    }

    public static void MummyMove()
    {
        foreach (WhiteMummy mummy in whiteMummies)
        {
            mummy.Move(playerPos);
        }
    }

    private bool playerCanMoveForward()
    {
        if (mapData[playerPos.y, playerPos.x, 0] == 1)
            return false;
        return true;
    }
    private bool playerCanMoveBackward()
    {
        // This check to make sure index is >= 0
        if (playerPos.y == 0)
            return false;
        if (mapData[playerPos.y - 1, playerPos.x, 0] == 1)
            return false;
        return true;
    }
    private bool playerCanMoveLeft()
    {
        // This check to make sure index is >= 0
        if (playerPos.x == 0)
            return false;
        if (mapData[playerPos.y, playerPos.x - 1, 3] == 1)
            return false;
        return true;
    }
    private bool playerCanMoveRight()
    {
        if (mapData[playerPos.y, playerPos.x, 3] == 1)
            return false;
        return true;
    }
}
