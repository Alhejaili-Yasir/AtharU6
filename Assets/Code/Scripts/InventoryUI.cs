using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI inventoryText;

    void Update()
    {
        inventoryText.text = "Inventory:\\n";

        foreach (var item in InventorySystem.Instance.items)
        {
            inventoryText.text += $"- {item.Key} × {item.Value}\\n";
        }
    }
}
