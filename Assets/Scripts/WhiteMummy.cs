using System.Collections.Generic;
using UnityEngine;

public class WhiteMummy
{
    private Vector2Int position;
    public static int[,,] mapData;

    [SerializeField]
    private GameObject mummyAsset;

    private List<Vector3> targetsList = new List<Vector3>();
    private GameObject mummy;
    private int firstMove = 0;

    public void SetPosition(Vector2Int position, GameObject mummy)
    {
        this.position = position;
        this.mummy = mummy;
        this.mummy.transform.position = new Vector3(position.x * 10f * Constant.Size, 10, position.y * 10f * Constant.Size);
    }

    public Vector2Int GetPosition()
    {
        return position;
    }

    public GameObject GetGameObject()
    {
        return mummy;
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

        // Second move
        if (PlayerPosition.x > position.x && canMoveRight())
            MoveRight();
        else if (PlayerPosition.x < position.x && canMoveLeft())
            MoveLeft();
        else if (PlayerPosition.y > position.y && canMoveUp())
            MoveUp();
        else if (PlayerPosition.y < position.y && canMoveDown())
            MoveDown();

        Vector3 pos = mummy.transform.position;
        pos.x = position.x * Constant.Size * 10;
        pos.z = position.y * Constant.Size * 10;
        mummy.transform.position = pos;
    }

    private Vector3 getNewStart()
    {
        Vector3 firstVec = Vector3.zero;
        switch (firstMove)
        {
            case 0: default:
                break;
            case 1: firstVec = Vector3.forward;
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

        return mummy.transform.position + firstVec;
    }

    private void MoveDown()
    {
        WorldManager.movingSomething = true;
        position.y--;
        if(firstMove == 0)
        {
            mummy.GetComponent<MovementComponent>().MoveBackward();
            firstMove = 3;
        }
        else
        {
            Vector3 start = getNewStart();
            mummy.GetComponent<MovementComponent>().MoveFromTo(start, start + Vector3.back * 10f * Constant.Size);
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
            mummy.GetComponent<MovementComponent>().MoveForward();
            firstMove = 1;
        }
        else
        {
            Vector3 start = getNewStart();
            mummy.GetComponent<MovementComponent>().MoveFromTo(start, start + Vector3.forward * 10f * Constant.Size);
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
            mummy.GetComponent<MovementComponent>().MoveLeft();
            firstMove = 2;
        }
        else
        {
            Vector3 start = getNewStart();
            mummy.GetComponent<MovementComponent>().MoveFromTo(start, start + Vector3.left * 10f * Constant.Size);
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
            mummy.GetComponent<MovementComponent>().MoveRight();
            firstMove = 4;
        }
        else
        {
            Vector3 start = getNewStart();
            mummy.GetComponent<MovementComponent>().MoveFromTo(start, start + Vector3.right * 10f * Constant.Size);
        }
    }

    private bool canMoveRight()
    {
        if (mapData[position.y, position.x, 3] == 1)
            return false;
        return true;
    }
}
