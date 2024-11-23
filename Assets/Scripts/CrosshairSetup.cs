using UnityEngine;
using UnityEngine.UI;

public class CrosshairSetup : MonoBehaviour
{
    public Image crosshairImage;

    private void Start()
    {
        if (crosshairImage == null)
        {
            Debug.LogError("Crosshair image not assigned!");
        }
    }
}
