using UnityEngine;
using System.Collections.Generic;

public class SimpleInventory : MonoBehaviour
{
    public static SimpleInventory Instance;

    private Dictionary<string, int> items = new Dictionary<string, int>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void AddItem(string itemName)
    {
        if (items.ContainsKey(itemName))
            items[itemName]++;
        else
            items[itemName] = 1;
    }

    public int GetItemCount(string itemName)
    {
        if (items.ContainsKey(itemName))
            return items[itemName];
        return 0;
    }

    public void RemoveItems(string itemName, int count)
    {
        if (items.ContainsKey(itemName))
        {
            items[itemName] -= count;
            if (items[itemName] <= 0)
                items.Remove(itemName);
        }
    }
}