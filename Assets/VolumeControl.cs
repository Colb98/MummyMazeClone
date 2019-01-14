using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    private void Awake()
    {
        Slider slider = gameObject.GetComponent<Slider>();
        if (slider != null)
        {
            slider.value = AudioListener.volume * 100f;
        }

        Toggle toggle = gameObject.GetComponent<Toggle>();
        if(toggle != null)
        {
            toggle.isOn = !AudioListener.pause;
        }
    }
}
