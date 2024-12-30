using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FriendlyNPCMovement : MonoBehaviour
{
    [SerializeField] private Animator npcAnimator;
    public Text Objectives;

    void Start()
    {
        FindObjectives();
    }

    // When you enter the crabs trigger zone the animation will start playing
    // This animation guides the player to the first part of the tutorial
    // This teaches them how to initially play the game
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npcAnimator.speed = 1f;
            npcAnimator.SetBool("startAnimation", true);
            FollowObjectives();

        }
    }

    // Animation stops when the player is too far away from the crab
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npcAnimator.speed = 0f;
        }
    }

    // Sets the objectives when ypu start the level
    private void FindObjectives()
    {
        Objectives.text = "Locate your crab friend near their house";
    }

    // Objectives change when you are following the crab
    private void FollowObjectives()
    {
        Objectives.text = "Follow your friend to locate the key";
    }


}