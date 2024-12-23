using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendNPCSpeech : MonoBehaviour
{
    public TextMeshProUGUI Speech; 
    private bool hasSpoken = false; 
    public TextMeshProUGUI Objectives;

    void Start()
    {
        StartSpeech();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && !hasSpoken)
        {
            StartCoroutine(ChangeSpeech());  
        }

    }

    private void StartSpeech()
    {
        Speech.text = "";
    }

    private IEnumerator ChangeSpeech()
    {
        hasSpoken = true;  

        Speech.text = "Hello Friend, I'm in need of some assistance";
        yield return new WaitForSeconds(3f);

        // Second part of speech
        Speech.text = "I have lost my last oxygen tank and this one's about to run out";
        yield return new WaitForSeconds(3f);

        Speech.text = "Please may you help and find it for me? I reckon that pufferfish has something to do with it";

        Objectives.text = "Help find the diver's last oxygen tank";
    }
}
