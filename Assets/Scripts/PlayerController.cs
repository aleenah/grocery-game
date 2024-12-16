using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float lookSpeed = 3f; 
    public float interactionRange = 6f; 
    public KeyCode pickupKey = KeyCode.E; 
    public Camera playerCamera; 
    public GameObject crosshair; 
    public GroceryGameManager gameManager; 
    public Sprite defaultCrosshairColor;
    public Sprite interactableCrosshairColor; 

    private Rigidbody rb;
    private GameObject targetItem; 
    private Image crosshairImage; 

    public TMP_Text timeText;
    public float timeLeft = 120;

    AudioSource src;
    public AudioClip pickupItem;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        src = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
        
        crosshairImage = crosshair.GetComponent<Image>();

        crosshair.SetActive(true);
        SetCrosshairColor(defaultCrosshairColor);
    }

    void Update()
    {
        HandleMovement();
        HandleLooking();
        HandleInteraction();

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Lose");
        }
        timeText.text = "Time: " + timeLeft.ToString("0.0");
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

        if (gameManager.groceryList.Contains(pickedItemName))
        {
            gameManager.groceryList.Remove(pickedItemName);
            src.PlayOneShot(pickupItem);

            Debug.Log($"Picked up item: {pickedItemName}");
            Debug.Log("Remaining items in grocery list:");

            foreach (string itemName in gameManager.groceryList)
            {
                Debug.Log(itemName); 
            }

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

        Destroy(item.gameObject);

        gameManager.CheckWinCondition();
    }



    void SetCrosshairColor(Sprite newCrosshair)
    {
        if (crosshairImage != null && crosshairImage.sprite != newCrosshair)
        {
            crosshairImage.sprite = newCrosshair;
        }
    }
}