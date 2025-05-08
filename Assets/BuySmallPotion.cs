using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemButton : MonoBehaviour
{
    public string itemName;
    public int price = 50;
    public bool isUsableItem = false; // ✅ إذا كان false = item للتجميع, true = للاستخدام

    public TextMeshProUGUI targetText; // ✅ يتم تعبئته من الـ Inspector
    public AudioClip purchaseSound;    // ✅ الصوت الذي يُشغل بعد الشراء

    private AudioSource audioSource;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(BuyItem);
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void BuyItem()
    {
        if (QuestManager.Instance.playerMoney >= price)
        {
            QuestManager.Instance.playerMoney -= price;

            if (isUsableItem)
            {
                QuestManager.Instance.AddShopItem(itemName);
                Debug.Log($"✅ Bought usable item: {itemName}");
            }
            else
            {
                QuestManager.Instance.AddItem(itemName);
                Debug.Log($"✅ Bought collectible item: {itemName}");
            }

            PlaySound();
            UpdateTargetText();
        }
        else
        {
            Debug.Log("❌ Not enough money!");
        }
    }

    void PlaySound()
    {
        if (purchaseSound != null)
            audioSource.PlayOneShot(purchaseSound);
    }

    void UpdateTargetText()
    {
        if (targetText == null) return;

        string display = "";
        var items = isUsableItem
            ? QuestManager.Instance.GetAllPurchasedItems()
            : QuestManager.Instance.GetAllCollectedItems();

        foreach (var entry in items)
        {
            display += $"{entry.Key}: {entry.Value}\n";
        }

        targetText.text = display;
    }
}
