using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inven : MonoBehaviour
{
    private Dictionary<int, ItemData> items = new Dictionary<int, ItemData>();

    public void AddItem(ItemData item)
    {
        if (items.ContainsKey(item.Key))
        {
            Debug.Log("Item already in inventory");
        }
        else
        {
            items.Add(item.Key, item);
            Debug.Log($"Added item: {item.ItemName}");
        }
    }

    public void RemoveItem(int key)
    {
        if (items.ContainsKey(key))
        {
            items.Remove(key);
            Debug.Log($"Removed item with key: {key}");
        }
        else
        {
            Debug.Log("Item not found in inventory");
        }
    }

    public ItemData GetItem(int key)
    {
        if (items.TryGetValue(key, out ItemData item))
        {
            return item;
        }
        else
        {
            Debug.Log("Item not found in inventory");
            return null;
        }
    }

    public void PrintInventory()
    {
        foreach (var item in items.Values)
        {
            Debug.Log($"Item: {item.ItemName}, Description: {item.Description}");
        }
    }
}