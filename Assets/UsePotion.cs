using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnifiedPotionUsage : MonoBehaviour
{
    [Header("Target Slider")]
    public Slider targetSlider;

    [Header("Potion Display Text")]
    public TextMeshProUGUI potionsText;

    [Header("Potion Names")]
    public string smallPotionName = "Small Potion";
    public string midPotionName = "Mid Potion";
    public string largePotionName = "Large Potion";

    [Header("Slider Add Values")]
    public float smallPotionValue = 15f;
    public float midPotionValue = 30f;
    public float largePotionValue = 50f;

    void OnEnable()
    {
        UpdatePotionText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            UsePotion(smallPotionName, smallPotionValue);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            UsePotion(midPotionName, midPotionValue);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            UsePotion(largePotionName, largePotionValue);
    }

    private void UsePotion(string itemName, float value)
    {
        Debug.Log($"Trying to use {itemName}");

        if (InventorySystem.Instance.HasItem(itemName))
        {
            Debug.Log($"{itemName} exists in inventory");

            InventorySystem.Instance.RemoveItem(itemName);
            targetSlider.value += value;

            Debug.Log($"Increased slider by {value}, new value: {targetSlider.value}");

            UpdatePotionText();
        }
        else
        {
            Debug.Log($"{itemName} NOT FOUND or count is 0");
        }
    }

    private void UpdatePotionText()
    {
        potionsText.text = "";

        foreach (var item in InventorySystem.Instance.items)
        {
            if (item.Key.Contains("Potion") && item.Value > 0)
            {
                potionsText.text += $"{item.Key}: {item.Value}\n";
            }
        }
    }
}
