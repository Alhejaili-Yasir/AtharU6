using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemButton : MonoBehaviour
{
    public string itemName;
    public int price = 50;
    public bool isUsableItem = false;

    public TextMeshProUGUI targetText;
    public AudioClip purchaseSound;

    [Header("Purchase Limits")]
    public int maxPurchaseCount = 99; // الحد الأقصى للشراء

    [Header("Toggle Objects on Purchase")]
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDeactivate;

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
        int currentCount = isUsableItem
            ? QuestManager.Instance.GetPurchasedAmount(itemName)
            : QuestManager.Instance.GetCollectedAmount(itemName);

        if (currentCount >= maxPurchaseCount)
        {
            Debug.Log($"❌ Can't buy more than {maxPurchaseCount} of {itemName}");
            return;
        }

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
            ToggleObjects();
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

    void ToggleObjects()
    {
        foreach (var obj in objectsToActivate)
        {
            if (obj != null) obj.SetActive(true);
        }

        foreach (var obj in objectsToDeactivate)
        {
            if (obj != null) obj.SetActive(false);
        }
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
