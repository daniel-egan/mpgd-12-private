using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendNPCSpeech : MonoBehaviour
{
    public TextMeshProUGUI Speech;  // Reference to the TextMeshPro component
    private bool hasSpoken = false; // To ensure speech happens only once
    public TextMeshProUGUI Objectives;

    void Start()
    {
        StartSpeech();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the player enters the trigger zone and speech hasn't been done yet, and the oxygen tank is not found
        if (other.CompareTag("Player") && !hasSpoken)
        {
            StartCoroutine(ChangeSpeech());  // Start the speech change coroutine
        }
        // If the tank has been found and speech hasn't happened yet, show the "Thank you!!!"
    }

    private void StartSpeech()
    {
        Speech.text = "";  // Initially empty speech
    }

    private IEnumerator ChangeSpeech()
    {
        hasSpoken = true;  // Mark that speech has been triggered

        Speech.text = "Hello Friend, I'm in need of some assistance";
        yield return new WaitForSeconds(3f);

        // Second part of speech
        Speech.text = "I have lost my last oxygen tank and this one's about to run out";
        yield return new WaitForSeconds(3f);

        Speech.text = "Please may you help and find it for me? I reckon that pufferfish has something to do with it";

        Objectives.text = "Help find the diver's last oxygen tank";
    }
}
