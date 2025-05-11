using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Quest
{
    public string questName;
    public string requiredItem;
    public int requiredAmount;
    public int rewardMoney;
    public bool isCompleted = false;
    public bool questAccepted = false;

    [Header("Shop Requirements (Optional)")]
    public string requiredShopItem;
    public int requiredShopItemAmount;

    [Header("GameObjects Activation")]
    public List<GameObject> activateOnStart;
    public List<GameObject> deactivateOnStart;

    public List<GameObject> activateOnComplete;
    public List<GameObject> deactivateOnComplete;
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public List<Quest> quests = new List<Quest>();
    public int currentQuestIndex = 0;
    public int playerMoney = 0;

    private Dictionary<string, int> collectedItems = new Dictionary<string, int>();
    private Dictionary<string, int> purchasedItems = new Dictionary<string, int>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        TriggerQuestStartEvents(); // لتفعيل أول مهمة
    }

    public Quest GetCurrentQuest()
    {
        if (currentQuestIndex < quests.Count)
            return quests[currentQuestIndex];
        return null;
    }

    public void MoveToNextQuest()
    {
        if (currentQuestIndex < quests.Count)
        {
            Quest current = quests[currentQuestIndex];

            // 🔁 تفعيل/إلغاء التفعيل بعد إنهاء المهمة
            foreach (var go in current.activateOnComplete)
                if (go != null) go.SetActive(true);

            foreach (var go in current.deactivateOnComplete)
                if (go != null) go.SetActive(false);
        }

        if (currentQuestIndex < quests.Count - 1)
        {
            currentQuestIndex++;
            TriggerQuestStartEvents(); // ✅ تفعيل بداية المهمة الجديدة
        }
    }

    public void TriggerQuestStartEvents()
    {
        Quest current = GetCurrentQuest();
        if (current == null) return;

        foreach (var go in current.activateOnStart)
            if (go != null) go.SetActive(true);

        foreach (var go in current.deactivateOnStart)
            if (go != null) go.SetActive(false);
    }

    public void AddItem(string itemName)
    {
        if (collectedItems.ContainsKey(itemName))
            collectedItems[itemName]++;
        else
            collectedItems[itemName] = 1;

        Debug.Log($"👜 Collected {itemName} (Total: {collectedItems[itemName]})");
    }

    public void AddPurchasedItem(string itemName)
    {
        if (purchasedItems.ContainsKey(itemName))
            purchasedItems[itemName]++;
        else
            purchasedItems[itemName] = 1;

        Debug.Log($"🛒 Purchased {itemName} (Total: {purchasedItems[itemName]})");
    }

    public void AddShopItem(string itemName)
    {
        AddPurchasedItem(itemName);
    }

    public int GetPurchasedAmount(string itemName)
    {
        return purchasedItems.ContainsKey(itemName) ? purchasedItems[itemName] : 0;
    }

    public bool HasRequiredShopItem(string itemName, int amount)
    {
        return purchasedItems.ContainsKey(itemName) && purchasedItems[itemName] >= amount;
    }

    public Dictionary<string, int> GetAllCollectedItems() => collectedItems;
    public Dictionary<string, int> GetAllPurchasedItems() => purchasedItems;

    public void RemoveItem(string itemName, int amount)
    {
        if (collectedItems.ContainsKey(itemName))
        {
            collectedItems[itemName] -= amount;
            if (collectedItems[itemName] <= 0)
                collectedItems.Remove(itemName);
        }
    }

    public int GetCollectedAmount(string itemName)
    {
        return collectedItems.ContainsKey(itemName) ? collectedItems[itemName] : 0;
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        Debug.Log($"💰 Money: {playerMoney}");
    }

    public bool SpendMoney(int amount)
    {
        if (playerMoney >= amount)
        {
            playerMoney -= amount;
            return true;
        }
        return false;
    }
}
