using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// used thehttps://www.youtube.com/watch?v=JS4k_lwmZHk
public class FriendlyNPCMovement : MonoBehaviour
{
    [SerializeField] private Animator npcAnimator;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npcAnimator.SetBool("startAnimation", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npcAnimator.SetBool("startAnimation", false);
        }
    }
}