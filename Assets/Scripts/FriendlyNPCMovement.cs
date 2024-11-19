using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FriendlyNPCMovement : MonoBehaviour
{
    [SerializeField] private Animator npcAnimator;
    public TextMeshProUGUI Objectives;
    void Start()
    {
        FindObjectives();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npcAnimator.speed = 1f;
            npcAnimator.SetBool("startAnimation", true);
            FollowObjectives();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npcAnimator.speed = 0f;
        }
    }
    private void FindObjectives()
    {
        Objectives.text = "Locate your crab friend";
    }

    private void FollowObjectives()
    {
        Objectives.text = "Follow your friend to locate the key";
    }


}