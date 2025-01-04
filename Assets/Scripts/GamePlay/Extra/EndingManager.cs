using UnityEngine;

public class EndingManager : MonoBehaviour
{
    // Singleton instance
    public static EndingManager Instance { get; private set; }

    // Object to activate
    [SerializeField] private GameObject objectToActivate;
    // Object to deactivate
    [SerializeField] private GameObject objectToDeactivate;

    private void Awake()
    {
        // Ensure singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Check if the all_tank achievement is unlocked at the start
        if (!CollectableManager.Instance.AreAllTanksCollected())
        {
            // If all_tank is not true, disable this component
            Debug.Log("All tanks have not been collected. EndingManager disabled.");
            this.enabled = false;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected.");
        // Toggle objects when any collision occurs
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }

        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
        }

        Debug.Log("Ending triggered. Objects toggled.");
    }
}
