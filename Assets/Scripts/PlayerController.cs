using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public float lookSpeed = 3f; // Look sensitivity
    public float interactionRange = 6f; // Range for interacting with items
    public KeyCode pickupKey = KeyCode.E; // Key to pick up items
    public Camera playerCamera; // Reference to the player's camera
    public GameObject crosshair; // Crosshair for aiming
    public GroceryGameManager gameManager; // Reference to the game manager
    public Sprite defaultCrosshairColor;
    public Sprite interactableCrosshairColor; 

    private Rigidbody rb;
    private GameObject targetItem; // Current item in view
    private Image crosshairImage; // Crosshair image component for color change

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the game window
        Cursor.visible = false; // Hide the cursor
        
        // Get the Image component from the crosshair object to change its color
        crosshairImage = crosshair.GetComponent<Image>();

        // Ensure crosshair is always visible and default color is set
        crosshair.SetActive(true);
        SetCrosshairColor(defaultCrosshairColor);
    }

    void Update()
    {
        HandleMovement();
        HandleLooking();
        HandleInteraction();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = (transform.forward * vertical + transform.right * horizontal).normalized;
        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
    }

    void HandleLooking()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        transform.Rotate(Vector3.up * mouseX);
        playerCamera.transform.localRotation *= Quaternion.Euler(-mouseY, 0f, 0f);
    }

    void HandleInteraction()
    {
        targetItem = null;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            GroceryItemComponent item = hit.collider.GetComponent<GroceryItemComponent>();
            if (item != null)
            {
                targetItem = hit.collider.gameObject;

                SetCrosshairColor(interactableCrosshairColor);

                // If the player presses the pickup key, handle the item pickup
                if (Input.GetKeyDown(pickupKey))
                {
                    HandlePickup(item);
                }
            }
            else
            {
                SetCrosshairColor(defaultCrosshairColor);
            }
        }
        else
        {
            SetCrosshairColor(defaultCrosshairColor);
        }
    }

    void HandlePickup(GroceryItemComponent item)
    {
        string pickedItemName = item.itemName;

        // Check if the item is on the grocery list
        if (gameManager.groceryList.Contains(pickedItemName))
        {
            gameManager.groceryList.Remove(pickedItemName);

            Debug.Log($"Picked up item: {pickedItemName}");
            Debug.Log("Remaining items in grocery list:");

            foreach (string itemName in gameManager.groceryList)
            {
                Debug.Log(itemName); // Print remaining items
            }

            // Update the UI to mark the item as collected
            foreach (Transform child in gameManager.groceryListPanel.transform)
            {
                GroceryUI ui = child.GetComponent<GroceryUI>();
                if (ui != null && ui.itemNameText.text == pickedItemName)
                {
                    ui.MarkAsCollected();
                    break;
                }
            }
        }

        // Destroy the item
        Destroy(item.gameObject);

        // Check if all items are collected
        CheckWinCondition();
    }



    void SetCrosshairColor(Sprite newCrosshair)
    {
        if (crosshairImage != null && crosshairImage.sprite != newCrosshair)
        {
            crosshairImage.sprite = newCrosshair;
        }
    }

    void CheckWinCondition()
    {
        if (gameManager.groceryList.Count == 0)
        {
            Debug.Log("You collected all the items! You win!");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Win");        }
    }
}
