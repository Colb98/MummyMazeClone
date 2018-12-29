using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHider : MonoBehaviour
{
    public GameObject player;
    private Transform lastObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            var renderers = lastObject.GetComponent<MeshRenderer>();
            renderers.enabled = true;
        }
        catch (System.Exception) { }

        RaycastHit hit;
        if (Physics.Linecast(transform.position, player.transform.position, out hit))
        {
            if (hit.transform.gameObject.tag != "Player")
            {
                lastObject = hit.transform;
                var renderer = lastObject.GetComponent<MeshRenderer>();
                renderer.enabled = false;
            }
        }
    }
}
