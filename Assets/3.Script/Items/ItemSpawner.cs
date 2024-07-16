using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<Item> items; 
    public int spawnCount = 1;  
    public Vector3 spawnArea = new Vector3(10, 0, 10); 

    void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnItem();
        }
    }

    void SpawnItem() {
        Item itemToSpawn = items[Random.Range(0, items.Count)];
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
            spawnArea.y +0.5f,
            Random.Range(-spawnArea.z / 2, spawnArea.z / 2)
        );

        if (itemToSpawn is CountableItem countItem) {
            countItem.resetCurrStack();
            countItem.addCurrStack(Random.Range(1, 8));
            GameObject itemObject = Instantiate(itemToSpawn.itemData.DropItemPrefab, randomPosition, Quaternion.identity);
            Item itemComponent = itemObject.GetComponent<Item>();
        }
        else {
            GameObject itemObject = Instantiate(itemToSpawn.itemData.DropItemPrefab, randomPosition, Quaternion.identity);
            Item itemComponent = itemObject.GetComponent<Item>();
        }
        //itemComponent.itemData = itemToSpawn;
        //if (itemComponent.itemData is FoodItemData foodItemData) {
        //    Debug.Log($"{foodItemData.ItemName} + {foodItemData.FullPoint}");
        //}
    }
}