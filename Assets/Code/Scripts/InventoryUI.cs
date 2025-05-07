using UnityEngine;
using TMPro;
using System.Text;

public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI inventoryText;
    public TextMeshProUGUI moneyText;
    public float updateInterval = 0.5f;

    void Start()
    {
        InvokeRepeating(nameof(UpdateUI), 0f, updateInterval);
    }

    void UpdateUI()
    {
        if (QuestManager.Instance == null)
            return;

        // ✅ تحديث الفلوس
        if (moneyText != null)
        {
            moneyText.text = $"Money: {QuestManager.Instance.playerMoney}";
        }

        // ✅ تحديث العناصر
        if (inventoryText != null)
        {
            var items = QuestManager.Instance.GetAllCollectedItems();
            StringBuilder sb = new StringBuilder();

            foreach (var entry in items)
            {
                sb.AppendLine($"{entry.Key}: {entry.Value}");
            }

            inventoryText.text = sb.ToString();
        }
    }
}
