using System.Collections.Generic;
using UnityEngine;

public class Scorpion
{
    private Vector2Int position;
    public static int[,,] mapData;

    [SerializeField]
    private GameObject scorpionAsset;

    private List<Vector3> targetsList = new List<Vector3>();
    private GameObject scorpion;
    private int firstMove = 0;

    public void SetPosition(Vector2Int position, GameObject scorpion)
    {
        this.position = position;
        this.scorpion = scorpion;
        Debug.Log(scorpion);
        this.scorpion.transform.position = new Vector3(position.x * 10f * Constant.Size, 10, position.y * 10f * Constant.Size);
    }

    public Vector2Int GetPosition()
    {
        return position;
    }

    public GameObject GetGameObject()
    {
        return scorpion;
    }

    public void Move(Vector2Int PlayerPosition)
    {
        // White mummies prior is the horizontal direction
        // First move
        firstMove = 0;
        if (PlayerPosition.x > position.x && canMoveRight())
            MoveRight();
        else if (PlayerPosition.x < position.x && canMoveLeft())
            MoveLeft();
        else if (PlayerPosition.y > position.y && canMoveUp())
            MoveUp();
        else if (PlayerPosition.y < position.y && canMoveDown())
            MoveDown();

        Vector3 pos = scorpion.transform.position;
        pos.x = position.x * Constant.Size * 10;
        pos.z = position.y * Constant.Size * 10;
        scorpion.transform.position = pos;
    }

    private Vector3 getNewStart()
    {
        Vector3 firstVec = Vector3.zero;
        switch (firstMove)
        {
            case 0:
            default:
                break;
            case 1:
                firstVec = Vector3.forward;
                break;
            case 2:
                firstVec = Vector3.left;
                break;
            case 3:
                firstVec = Vector3.back;
                break;
            case 4:
                firstVec = Vector3.right;
                break;
        }

        firstVec *= 10f * Constant.Size;

        return scorpion.transform.position + firstVec;
    }

    private void MoveDown()
    {
        WorldManager.movingSomething = true;
        position.y--;
        if (firstMove == 0)
        {
            scorpion.GetComponent<MovementComponent>().MoveBackward();
            firstMove = 3;
        }
        else
        {
            Vector3 start = getNewStart();
            scorpion.GetComponent<MovementComponent>().MoveFromTo(start, start + Vector3.back * 10f * Constant.Size, new Vector3(0,180,0));
        }
    }

    private bool canMoveDown()
    {
        if (position.y == 0)
            return false;
        if (mapData[position.y - 1, position.x, 0] == 1)
            return false;
        return true;
    }

    private void MoveUp()
    {
        WorldManager.movingSomething = true;
        position.y++;
        if (firstMove == 0)
        {
            scorpion.GetComponent<MovementComponent>().MoveForward();
            firstMove = 1;
        }
        else
        {
            Vector3 start = getNewStart();
            scorpion.GetComponent<MovementComponent>().MoveFromTo(start, start + Vector3.forward * 10f * Constant.Size, new Vector3(0, 0, 0));
        }
    }

    private bool canMoveUp()
    {
        if (mapData[position.y, position.x, 0] == 1)
            return false;
        return true;
    }

    private void MoveLeft()
    {
        WorldManager.movingSomething = true;
        position.x--;
        if (firstMove == 0)
        {
            scorpion.GetComponent<MovementComponent>().MoveLeft();
            firstMove = 2;
        }
        else
        {
            Vector3 start = getNewStart();
            scorpion.GetComponent<MovementComponent>().MoveFromTo(start, start + Vector3.left * 10f * Constant.Size, new Vector3(0, -90, 0));
        }
    }

    private bool canMoveLeft()
    {
        if (position.x == 0)
            return false;
        if (mapData[position.y, position.x - 1, 3] == 1)
            return false;
        return true;
    }

    private void MoveRight()
    {
        WorldManager.movingSomething = true;
        position.x++;
        if (firstMove == 0)
        {
            scorpion.GetComponent<MovementComponent>().MoveRight();
            firstMove = 4;
        }
        else
        {
            Vector3 start = getNewStart();
            scorpion.GetComponent<MovementComponent>().MoveFromTo(start, start + Vector3.right * 10f * Constant.Size, new Vector3(0, 90, 0));
        }
    }

    private bool canMoveRight()
    {
        if (mapData[position.y, position.x, 3] == 1)
            return false;
        return true;
    }
}
