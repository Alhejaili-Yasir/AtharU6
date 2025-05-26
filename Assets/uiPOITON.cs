using UnityEngine;
using TMPro;

public class PotionUIElement : MonoBehaviour
{
    [Header("Potion Settings")]
    public string potionName;

    [Header("UI References")]
    public TextMeshProUGUI countText;

    void Update()
    {
        int count = InventorySystem.Instance.GetItemCount(potionName);
        countText.text = count.ToString();
    }
}
