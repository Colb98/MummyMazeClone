using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionWhilePlaying : MonoBehaviour
{
    Canvas optionPanel = null;
    public Canvas optionPanelPrefab;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPanel();
        }
    }

    public void ShowPanel()
    {
        WorldManager.Paused = true;
        optionPanel = Instantiate(optionPanelPrefab);
    }
}
