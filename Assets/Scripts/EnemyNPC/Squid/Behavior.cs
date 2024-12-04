using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    public Animator npcAnimator; 


    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            npcAnimator.SetTrigger("Attack"); 
        }
    }


    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            npcAnimator.SetTrigger("Swim");  
        }
    }
}
