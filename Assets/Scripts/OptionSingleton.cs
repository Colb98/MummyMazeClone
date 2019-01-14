using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSingleton : MonoBehaviour
{
    private static OptionSingleton instance = null;
    public static OptionSingleton Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
