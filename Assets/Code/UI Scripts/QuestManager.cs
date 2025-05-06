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
}




public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public List<Quest> quests = new List<Quest>();
    public int currentQuestIndex = 0;
    public int playerMoney = 0;

    private Dictionary<string, int> collectedItems = new Dictionary<string, int>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public Quest GetCurrentQuest()
    {
        if (currentQuestIndex < quests.Count)
            return quests[currentQuestIndex];
        return null;
    }

    public void MoveToNextQuest()
    {
        if (currentQuestIndex < quests.Count - 1)
            currentQuestIndex++;
    }

    public void AddItem(string itemName)
    {
        if (collectedItems.ContainsKey(itemName))
            collectedItems[itemName]++;
        else
            collectedItems[itemName] = 1;

        Debug.Log($"👜 Collected {itemName} (Total: {collectedItems[itemName]})");
    }

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

    public Dictionary<string, int> GetAllCollectedItems()
    {
        return collectedItems;
    }
}
