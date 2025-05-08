using UnityEngine;

public class InventoryToggleUI : MonoBehaviour
{
    public GameObject inventoryPanel;         // UI panel to toggle
    public GameObject[] objectsToDisable;     // Objects to deactivate when inventory is open

    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpen = !isOpen;
            inventoryPanel.SetActive(isOpen);

            foreach (GameObject obj in objectsToDisable)
            {
                if (obj != null)
                    obj.SetActive(!isOpen);
            }
        }
    }
}
