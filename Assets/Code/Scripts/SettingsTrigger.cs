using UnityEngine;

public class SettingsMenuToggle : MonoBehaviour
{
    public static bool IsSettingsOpen { get; private set; }

    public GameObject settingsCanvas;
    public GameObject[] objectsToHide;
    public MonoBehaviour[] scriptsToDisable;

    private bool isMenuOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingsMenu();
        }
    }

    void ToggleSettingsMenu()
    {
        isMenuOpen = !isMenuOpen;
        IsSettingsOpen = isMenuOpen;

        if (settingsCanvas != null)
            settingsCanvas.SetActive(isMenuOpen);

        foreach (GameObject obj in objectsToHide)
            if (obj != null) obj.SetActive(!isMenuOpen);

        foreach (MonoBehaviour script in scriptsToDisable)
            if (script != null) script.enabled = !isMenuOpen;

        Time.timeScale = isMenuOpen ? 0f : 1f;

        Cursor.visible = isMenuOpen;
        Cursor.lockState = isMenuOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
