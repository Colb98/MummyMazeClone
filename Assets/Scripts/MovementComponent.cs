using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    List<Vector3> startQueue = new List<Vector3>();
    [SerializeField]
    List<Vector3> targetQueue = new List<Vector3>();
    List<string> MoveDirections = new List<string>();
    const float timeToReach = 1;
    float t = 2;
    bool justMoved = false;
    public Animation anim;
    Vector3 waitAngle;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        Debug.Log(anim);
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
            if(waitAngle != new Vector3(5,5,5))
            {
                transform.eulerAngles = waitAngle;
                waitAngle = new Vector3(5,5,5);
            }
        }
        else if (justMoved)
        {
            float x = transform.position.x;
            float y = transform.position.y;
            float z = transform.position.z;
            x = Mathf.Round(x / 20) * 20;
            z = Mathf.Round(z / 20) * 20;
            transform.SetPositionAndRotation(new Vector3(x, y, z), transform.rotation);
            Debug.Log(x.ToString() + "," + y.ToString() + "," + z.ToString());
            justMoved = false;
            targetQueue.Clear();
            startQueue.Clear();
            WorldManager.movingSomething = false;
            if (gameObject.tag == "Player")
            {
                WorldManager.MummyMove();
                GetComponent<Animator>().SetBool("Walk", false);
            }
            else if (gameObject.name.Contains("Mummy") || gameObject.name.Contains("Spider"))
                GetComponent<Animator>().SetBool("Idle", true);
        }
    }

    public void StopMoving()
    {
        startQueue.Clear();
        targetQueue.Clear();
        justMoved = false;
        WorldManager.movingSomething = false;
        t = 2;
    }

    public void MoveLeft()
    {
        Debug.Log("Left");
        if (targetQueue.Count > 0)
            return;
        t = 0;
        targetQueue.Add(transform.position + Vector3.left * Constant.Size * 10f);
        startQueue.Add(transform.position);
        transform.eulerAngles = new Vector3(0, -90, 0);
        if (gameObject.name.Contains("Player"))
        {
            WorldManager.playerPos.x--;
            GetComponent<Animator>().SetBool("Walk", true);
        }
        else if (gameObject.name.Contains("Mummy") || gameObject.name.Contains("Spider"))
            GetComponent<Animator>().SetBool("Idle", false);
    }

    public void MoveRight()
    {
        Debug.Log("right");
        if (targetQueue.Count > 0)
            return;
        t = 0;
        targetQueue.Add(transform.position + Vector3.right * Constant.Size * 10f);
        startQueue.Add(transform.position);        
        transform.eulerAngles = new Vector3(0, 90, 0);
        if (gameObject.name.Contains("Player"))
        {
            WorldManager.playerPos.x++;
            GetComponent<Animator>().SetBool("Walk", true);
        }
        else if (gameObject.name.Contains("Mummy") || gameObject.name.Contains("Spider"))
            GetComponent<Animator>().SetBool("Idle", false);
    }

    public void MoveForward()
    {
        Debug.Log("up");
        if (targetQueue.Count > 0)
            return;
        t = 0;
        targetQueue.Add(transform.position + Vector3.forward * Constant.Size * 10f);
        startQueue.Add(transform.position);
        transform.eulerAngles = new Vector3(0, 0, 0);

        if (gameObject.name.Contains("Player"))
        {
            WorldManager.playerPos.y++;
            GetComponent<Animator>().SetBool("Walk", true);
        }
        else if (gameObject.name.Contains("Mummy") || gameObject.name.Contains("Spider"))
            GetComponent<Animator>().SetBool("Idle", false);
    }

    public void MoveBackward()
    {
        Debug.Log("Down");
        if (targetQueue.Count > 0)
            return;
        t = 0;
        targetQueue.Add(transform.position + Vector3.back * Constant.Size * 10f);
        startQueue.Add(transform.position);
        transform.eulerAngles = new Vector3(0, 180, 0);
        if (gameObject.name.Contains("Player"))
        {
            WorldManager.playerPos.y--;
            GetComponent<Animator>().SetBool("Walk", true);
        }
        else if (gameObject.name.Contains("Mummy") || gameObject.name.Contains("Spider"))
            GetComponent<Animator>().SetBool("Idle", false);
    }

    public void MoveFromTo(Vector3 startPos, Vector3 endPos, Vector3 angle)
    {
        if (targetQueue.Count > 2)
            return;
        waitAngle = angle;
        targetQueue.Add(endPos);
        startQueue.Add(startPos);
    }
}
