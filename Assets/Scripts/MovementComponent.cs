using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    List<Vector3> startQueue = new List<Vector3>();
    [SerializeField]
    List<Vector3> targetQueue = new List<Vector3>();
    const float timeToReach = 1;
    float t = 2;
    bool justMoved = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (t < 1)
        {
            justMoved = true;
            t += Time.deltaTime / timeToReach;
            transform.position = Vector3.Lerp(startQueue[0], targetQueue[0], t);
        }  
        else if (targetQueue.Count > 1)
        {
            t = 0;
            targetQueue.RemoveAt(0);
            startQueue.RemoveAt(0);
        }
        else if (justMoved)
        {
            justMoved = false;
            targetQueue.Clear();
            startQueue.Clear();
            WorldManager.movingSomething = false;
            if (gameObject.tag == "Player")
                WorldManager.MummyMove();
        }
    }

    public void MoveLeft()
    {
        if (targetQueue.Count > 0)
            return;
        t = 0;
        targetQueue.Add(transform.position + Vector3.left * Constant.Size * 10f);
        startQueue.Add(transform.position);
    }

    public void MoveRight()
    {
        if (targetQueue.Count > 0)
            return;
        t = 0;
        targetQueue.Add(transform.position + Vector3.right * Constant.Size * 10f);
        startQueue.Add(transform.position);
    }

    public void MoveForward()
    {
        if (targetQueue.Count > 0)
            return;
        t = 0;
        targetQueue.Add(transform.position + Vector3.forward * Constant.Size * 10f);
        startQueue.Add(transform.position);
    }

    public void MoveBackward()
    {
        if (targetQueue.Count > 0)
            return;
        t = 0;
        targetQueue.Add(transform.position + Vector3.back * Constant.Size * 10f);
        startQueue.Add(transform.position);
    }

    public void MoveFromTo(Vector3 startPos, Vector3 endPos)
    {
        if (targetQueue.Count > 2)
            return;
        targetQueue.Add(endPos);
        startQueue.Add(startPos);
    }
}
