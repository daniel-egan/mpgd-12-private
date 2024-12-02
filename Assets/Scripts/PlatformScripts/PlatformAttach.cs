using UnityEngine;

public class PlatformAttachment : MonoBehaviour
{
    private Transform originalParent; // To store the original parent of the player

    void Start()
    {
        // Save the player's initial parent in case we need to reset
        originalParent = transform.parent;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            Debug.Log("TagFound: Attaching to platform.");
            // Set the player's parent to the platform
            transform.SetParent(collision.transform);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            Debug.Log("TagLost: Detaching from platform.");
            // Reset the player's parent to the original
            transform.SetParent(originalParent);
        }
    }
}
