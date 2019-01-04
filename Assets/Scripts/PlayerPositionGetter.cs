using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionGetter : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        string name = collision.gameObject.name;
        if (name.Contains("Floor"))
        {
            name = name.Remove(0, 6);
            name = name.Remove(name.Length - 1);
            string[] bits = name.Split(',');
            WorldManager.playerPos.x = int.Parse(bits[1]);
            WorldManager.playerPos.y = int.Parse(bits[0]);

            Debug.Log(WorldManager.playerPos);
        }
    }
}
