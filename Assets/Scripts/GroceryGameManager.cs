using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroceryGameManager : MonoBehaviour
{
    public List<GameObject> groceryItemPrefabs; // Prefabs of grocery items
    public List<Transform> spawnPoints; // Spawn points for items
    public int minItemsPerType = 1; // Minimum items per type
    public int maxItemsPerType = 5; // Maximum items per type
    public int itemsToCollect = 5; // Number of items to collect for the grocery list

    public GameObject groceryUIPrefab; // Prefab for a single grocery list UI item
    public GameObject groceryListPanel; // The panel where the grocery list UI items will appear

    public List<string> groceryList = new List<string>(); // The list of grocery item names to collect

    void Start()
    {
        GenerateGroceryItems();
        GenerateGroceryList();
    }

    void GenerateGroceryItems()
    {
        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

        foreach (GameObject prefab in groceryItemPrefabs)
        {
            int itemCount = Random.Range(minItemsPerType, maxItemsPerType + 1);

            for (int i = 0; i < itemCount; i++)
            {
                if (availableSpawnPoints.Count == 0)
                {
                    Debug.LogWarning("Not enough spawn points for all items!");
                    return;
                }

                int spawnIndex = Random.Range(0, availableSpawnPoints.Count);
                Transform spawnPoint = availableSpawnPoints[spawnIndex];
                availableSpawnPoints.RemoveAt(spawnIndex);

                Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            }
        }
    }

    void GenerateGroceryList()
    {
        // Randomly select items for the grocery list
        List<GameObject> shuffledItems = groceryItemPrefabs.OrderBy(_ => Random.value).ToList();

        for (int i = 0; i < Mathf.Min(itemsToCollect, shuffledItems.Count); i++)
        {
            GroceryItemComponent itemComponent = shuffledItems[i].GetComponent<GroceryItemComponent>();
            if (itemComponent != null)
            {
                groceryList.Add(itemComponent.itemName);

                // Create a UI element for this item
                GameObject uiElement = Instantiate(groceryUIPrefab, groceryListPanel.transform);
                GroceryUI groceryUI = uiElement.GetComponent<GroceryUI>();
                groceryUI.Setup(itemComponent.itemName, itemComponent.itemIcon);
            }
        }
    }
}
