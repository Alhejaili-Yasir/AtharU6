using System.Collections.Generic;
using UnityEngine;

public class UIToggleHandler : MonoBehaviour
{
    [Header("UI to Monitor")]
    public GameObject uiPanel; // UI اللي نراقب حالته

    [Header("Objects to Deactivate")]
    public List<GameObject> objectsToDeactivate;

    [Header("Scripts to Disable")]
    public List<MonoBehaviour> scriptsToDisable;

    private bool lastUIState;

    void Start()
    {
        lastUIState = uiPanel.activeSelf;
        HandleToggle(lastUIState);
    }

    void Update()
    {
        if (uiPanel.activeSelf != lastUIState)
        {
            lastUIState = uiPanel.activeSelf;
            HandleToggle(lastUIState);
        }
    }

    void HandleToggle(bool isUIActive)
    {
        bool deactivateOthers = isUIActive;

        foreach (var obj in objectsToDeactivate)
        {
            if (obj != null)
                obj.SetActive(!deactivateOthers);
        }

        foreach (var script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = !deactivateOthers;
        }
    }
}
