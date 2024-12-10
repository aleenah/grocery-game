using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class GroceryGameManager : MonoBehaviour
{
    public List<GameObject> groceryItemPrefabs; 
    public List<Transform> spawnPoints; 
    public int minItemsPerType = 1; 
    public int maxItemsPerType = 5; 
    public int startingItemsToCollect = 5; 
    public int itemsIncrement = 2;
    public int levels = 3;

    public GameObject groceryUIPrefab; 
    public GameObject groceryListPanel; 

    private int currentLevel = 1;
    private int itemsToCollect;

    public TMP_Text levelText;

    public List<string> groceryList = new List<string>(); 

    void Start() 
    {
        UpdateLevelText();
        itemsToCollect = startingItemsToCollect;
        StartLevel();
    }

    void StartLevel()
    {
        UpdateLevelText();
        ClearExistingItems();
        GenerateGroceryItems();
        GenerateGroceryList();
    }

    void UpdateLevelText()
    {
    levelText.text = "Level: " + currentLevel;
    }

    void ClearExistingItems()
    {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("GroceryItem"))
        {
            Destroy(item);
        }

        foreach (Transform child in groceryListPanel.transform)
        {
            Destroy(child.gameObject);
        }

        groceryList.Clear();
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
        List<GameObject> shuffledItems = groceryItemPrefabs.OrderBy(_ => Random.value).ToList();

        for (int i = 0; i < Mathf.Min(itemsToCollect, shuffledItems.Count); i++)
        {
            GroceryItemComponent itemComponent = shuffledItems[i].GetComponent<GroceryItemComponent>();
            if (itemComponent != null)
            {
                groceryList.Add(itemComponent.itemName);

                GameObject uiElement = Instantiate(groceryUIPrefab, groceryListPanel.transform);
                GroceryUI groceryUI = uiElement.GetComponent<GroceryUI>();
                groceryUI.Setup(itemComponent.itemName, itemComponent.itemIcon);
            }
        }
    }

    public void CheckWinCondition()
    {
        if (groceryList.Count == 0)
        {
            if (currentLevel < levels)
            {
                currentLevel++;
                itemsToCollect += itemsIncrement;
                StartLevel();
            } else {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Win");
            }
        }
    }
}
