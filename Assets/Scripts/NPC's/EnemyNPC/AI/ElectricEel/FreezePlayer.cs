using System.Collections;
using UnityEngine;

public class FreezePlayer : MonoBehaviour
{
    public GameObject player;
    public GameObject eel;
    public float npcFreezeDuration = 5f;
    private CharacterController playerController;
    private BoxCollider boxCollider;
    public Canvas splashScreen;
    private bool isNpcFrozen = false;



    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (player != null)
        {
            playerController = player.GetComponent<CharacterController>();
        }
    }

    // Calls the freeze function when the player enters the electric eels trigger zone
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FreezePlayerMovement();
        }
    }

    // When the player is within the electric eels trigger zone the character controller for the player is turned off
    // This results in them not being able to move
    // The electric bolt sign also is displayed on the screen
    // The electric eels ability to freeze the player is alos disabled
    void FreezePlayerMovement()
    {
        if (playerController != null)
        {
            if (isNpcFrozen == false)
            {
                playerController.enabled = false;
                splashScreen.gameObject.SetActive(true);
                boxCollider.isTrigger = false;
                StartCoroutine(UnfreezePlayerCoroutine());
                
            }
        }
    }
    // Timer for the duration of how long the player will be frozen for
    IEnumerator UnfreezePlayerCoroutine()
    {
        yield return new WaitForSeconds(npcFreezeDuration); 
        splashScreen.gameObject.SetActive(false);
        UnfreezePlayer();
    }

    // Returns movement ability back to the player
    public void UnfreezePlayer()
    {
        if (playerController != null)
        {
            playerController.enabled = true;
            StartCoroutine(EnableBoxCollider());
        }
    }
    // returns freeze ability back to the npc
    IEnumerator EnableBoxCollider()
    {
        yield return new WaitForSeconds(npcFreezeDuration);
        boxCollider.isTrigger = true;
    }

}
