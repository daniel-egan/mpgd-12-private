// Code used from CHATGPT
using UnityEngine;

public class ExpandSphere : MonoBehaviour
{
    public float expansionSpeed = 2.0f; 
    public float maxScale = 3.0f;       
    private bool isExpanding = false;   
    private Vector3 originalScale;      

    void Start()
    {
        originalScale = transform.localScale;;
    }

    // Expands the puffer fish gradually when the player is in range
    // Also the pufferfish shrinks back to its original size when the player is out of the range
    void Update()
    {
        if (isExpanding && transform.localScale.x < maxScale)
        {
            transform.localScale += Vector3.one * expansionSpeed * Time.deltaTime;
        }
        else if (!isExpanding && transform.localScale.x > originalScale.x)
        {
            transform.localScale -= Vector3.one * expansionSpeed * Time.deltaTime;
            if (transform.localScale.x < originalScale.x)
            {
                transform.localScale = originalScale;
            }
        }
    }

    // Check if the player enters the trigger and sets expanding to true
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isExpanding = true;
        }
    }

    // Stops expanding when the player exits the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isExpanding = false;
        }
    }
}
