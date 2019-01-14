﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    public void DestroySelf()
    {
        Destroy(this.gameObject);
        WorldManager.Paused = false;
    }
}