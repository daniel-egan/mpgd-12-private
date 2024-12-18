using System.Collections;
using UnityEngine;

public class FreezePlayer : MonoBehaviour
{
    // Reference to the player GameObject
    public GameObject player;

    // The duration for which to freeze the NPC (you can adjust this)
    public float npcFreezeDuration = 5f;

    // Cached components
    private CharacterController playerController;
    private Rigidbody npcRigidbody;

    public Canvas splashScreen;

    private bool isNpcFrozen = false;

    void Start()
    {
        // Ensure the player is assigned and get necessary components
        if (player != null)
        {
            playerController = player.GetComponent<CharacterController>();
        }

        npcRigidbody = GetComponent<Rigidbody>(); // This assumes the NPC has a Rigidbody
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Freeze the player and NPC upon trigger
            FreezePlayerMovement();
            FreezeNpcMovement();
        }
    }

    // Freezes the player's movement
    void FreezePlayerMovement()
    {
        if (playerController != null)
        {
            playerController.enabled = false; // Disable the player's movement (if using CharacterController)
            splashScreen.gameObject.SetActive(true);
            StartCoroutine(UnfreezePlayerCoroutine());
        }
    }

    // Freezes the NPC's movement
    void FreezeNpcMovement()
    {
        if (npcRigidbody != null)
        {
            npcRigidbody.velocity = Vector3.zero; // Stop any ongoing motion
            npcRigidbody.isKinematic = true; // Disable physics interaction for the NPC
            isNpcFrozen = true;

            // Start a coroutine to unfreeze the NPC after some time
            StartCoroutine(UnfreezeNPCCoroutine());
            
        }
    }

    // Coroutine to unfreeze the NPC after the specified duration
    IEnumerator UnfreezePlayerCoroutine()
    {
        yield return new WaitForSeconds(npcFreezeDuration); // Wait for the freeze duration
        splashScreen.gameObject.SetActive(false);
        UnfreezePlayer();
    }

    IEnumerator UnfreezeNPCCoroutine()
    {
        yield return new WaitForSeconds(npcFreezeDuration*2); // Wait for the freeze duration
        UnfreezeNpc();
    }

    // Unfreezes the NPC's movement
    void UnfreezeNpc()
    {
        if (npcRigidbody != null)
        {
            npcRigidbody.isKinematic = false; // Re-enable physics
            isNpcFrozen = false;
        }
    }

    // Optionally, you can add a method to unfreeze the player after a certain time
    public void UnfreezePlayer()
    {
        if (playerController != null)
        {
            playerController.enabled = true; // Re-enable the player's movement
        }
    }
}
