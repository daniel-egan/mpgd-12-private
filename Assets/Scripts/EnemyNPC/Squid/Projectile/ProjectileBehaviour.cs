using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 direction; // The direction the projectile will move

    public Canvas hitCanvas; // Reference to the Canvas to be shown
    public float effectDuration = 3f; // Duration the effect stays visible

    public void Initialize(Transform target)
    {
        // Calculate the direction once at the start
        direction = (target.position - transform.position).normalized;
    }

    void Start()
    {
    }

    void Update()
    {
        // Move the projectile in the predefined direction
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit!");
            Destroy(gameObject);

            // Check if the canvas reference is assigned
            if (hitCanvas == null)
            {
                Debug.LogError("HitCanvas is not assigned!");
            }
            else
            {
                // Show the Canvas for the effect
                hitCanvas.gameObject.SetActive(true);
                Debug.Log("Canvas activated!");
            }

            // Optional: You can wait a moment to deactivate the canvas
            StartCoroutine(HideCanvasAfterDelay(3f));  // Hide the canvas after 3 seconds
        }
    }

    private IEnumerator HideCanvasAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Disable the Canvas after delay
        if (hitCanvas != null)
        {
            hitCanvas.gameObject.SetActive(false);
            Debug.Log("Canvas deactivated!");
        }
    }

}


