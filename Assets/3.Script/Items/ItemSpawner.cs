using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<ItemData> items;  // List of all possible items to spawn
    public int spawnCount = 5;    // Number of items to spawn
    public Vector3 spawnArea = new Vector3(10, 0, 10);  // Define the area to spawn items

    void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnItem();
        }
    }

    void SpawnItem()
    {
        ItemData itemToSpawn = items[Random.Range(0, items.Count)];
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
            spawnArea.y,
            Random.Range(-spawnArea.z / 2, spawnArea.z / 2)
        );

        GameObject itemObject = Instantiate(itemToSpawn.DropItemPrefab, randomPosition, Quaternion.identity);        itemObject.GetComponent<ItemPickup>().itemData = itemToSpawn;
    }
}