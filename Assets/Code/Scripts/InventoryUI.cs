using UnityEngine;
using TMPro;
using System.Text;

public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI collectedItemsText;
    public TextMeshProUGUI purchasedItemsText;
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

        if (moneyText != null)
            moneyText.text = $"{QuestManager.Instance.playerMoney}";

        if (collectedItemsText != null)
        {
            var items = QuestManager.Instance.GetAllCollectedItems();
            StringBuilder sb = new StringBuilder();
            foreach (var entry in items)
                sb.AppendLine($"{entry.Key}: {entry.Value}");
            collectedItemsText.text = sb.ToString();
        }

        if (purchasedItemsText != null)
        {
            var items = QuestManager.Instance.GetAllPurchasedItems();
            StringBuilder sb = new StringBuilder();
            foreach (var entry in items)
                sb.AppendLine($"{entry.Key}: {entry.Value}");
            purchasedItemsText.text = sb.ToString();
        }
    }
}
