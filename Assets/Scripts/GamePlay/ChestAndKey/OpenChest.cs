using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{
    [SerializeField] private Animator npcAnimator;


    // When the player collides with the chest the open animation starts, reveaing the key
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            npcAnimator.SetTrigger("Open");
        }
    }
}
