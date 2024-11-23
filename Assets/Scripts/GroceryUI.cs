using UnityEngine;
using UnityEngine.UI;
using TMPro; // Ensure this is included

public class GroceryUI : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;  // Reference to TextMeshProUGUI
    public Image itemIconImage;  // Reference to Image for the grocery item icon

    public void Setup(string itemName, Sprite itemIcon)
    {
        itemNameText.text = itemName; // Set the item name
        itemIconImage.sprite = itemIcon; // Set the item icon
    }

    public void MarkAsCollected()
    {
        itemNameText.color = Color.gray;
        itemIconImage.color = new Color(1f, 1f, 1f, 0.5f); // Reduce opacity
    }
}

