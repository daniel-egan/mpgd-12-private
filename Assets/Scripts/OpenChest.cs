using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{
    [SerializeField] private Animator npcAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // if the player collides with the object
        {
            npcAnimator.SetTrigger("Open");
        }
    }
}
