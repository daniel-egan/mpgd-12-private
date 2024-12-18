using UnityEngine;

public class ExpandSphere : MonoBehaviour
{
    public float expansionSpeed = 2.0f; // How fast the sphere expands
    public float maxScale = 3.0f;       // Maximum size the sphere can reach
    private bool isExpanding = false;   // Check if the sphere should expand
    private Vector3 originalScale;      // Store the initial scale of the sphere

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isExpanding && transform.localScale.x < maxScale)
        {
            // Expand the sphere gradually
            transform.localScale += Vector3.one * expansionSpeed * Time.deltaTime;
        }
        else if (!isExpanding && transform.localScale.x > originalScale.x)
        {
            // Shrink the sphere back to its original size
            transform.localScale -= Vector3.one * expansionSpeed * Time.deltaTime;

            // Clamp to the original size to avoid overshooting
            if (transform.localScale.x < originalScale.x)
            {
                transform.localScale = originalScale;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger
        if (other.CompareTag("Player"))
        {
            isExpanding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Optional: Stop expanding when the player exits the trigger
        if (other.CompareTag("Player"))
        {
            isExpanding = false;
        }
    }
}
