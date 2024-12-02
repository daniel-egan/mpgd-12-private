using UnityEngine;
using UnityEngine.InputSystem;  // Make sure to add this namespace

public class CursorSwitcher : MonoBehaviour
{
    public GameObject customCursorObject; // Assign your canvas cursor object here

    private bool defaultCursorVisible = true;
    private Mouse mouse;

    void Start()
    {
        // Get the mouse input device
        mouse = Mouse.current;

        // Start with the Unity cursor visible and custom cursor hidden
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        customCursorObject.SetActive(false);
    }

    void Update()
    {
        // Use the new Input System to check for mouse click
        if (mouse.leftButton.wasPressedThisFrame && defaultCursorVisible)
        {
            // Hide default cursor and show custom cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center
            customCursorObject.SetActive(true);
            defaultCursorVisible = false;
        }
        else if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // Show default cursor and hide custom cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            customCursorObject.SetActive(false);
            defaultCursorVisible = true;
        }
    }
}
