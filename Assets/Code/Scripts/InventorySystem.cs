using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    // Dictionary لتخزين العنصر وعدده
    public Dictionary<string, int> items = new Dictionary<string, int>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddItem(string itemName)
    {
        if (items.ContainsKey(itemName))
        {
            items[itemName]++;
        }
        else
        {
            items[itemName] = 1;
        }

        Debug.Log($"تمت إضافة: {itemName} (العدد: {items[itemName]})");
    }

    public void RemoveItem(string itemName)
    {
        if (items.ContainsKey(itemName))
        {
            items[itemName]--;

            if (items[itemName] <= 0)
                items.Remove(itemName);
        }
    }

    public bool HasItem(string itemName, int requiredAmount = 1)
    {
        return items.ContainsKey(itemName) && items[itemName] >= requiredAmount;
    }

    public int GetItemCount(string itemName)
    {
        return items.ContainsKey(itemName) ? items[itemName] : 0;
    }
}
