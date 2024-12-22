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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FreezePlayerMovement();
        }
    }

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
    IEnumerator UnfreezePlayerCoroutine()
    {
        yield return new WaitForSeconds(npcFreezeDuration); 
        splashScreen.gameObject.SetActive(false);
        UnfreezePlayer();
    }

    public void UnfreezePlayer()
    {
        if (playerController != null)
        {
            playerController.enabled = true;
            StartCoroutine(EnableBoxCollider());
        }
    }

    IEnumerator EnableBoxCollider()
    {
        yield return new WaitForSeconds(npcFreezeDuration);
        boxCollider.isTrigger = true;
    }

}
