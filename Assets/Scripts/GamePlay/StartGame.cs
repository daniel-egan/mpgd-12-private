using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartGame : MonoBehaviour
{
    private int hasSpoken = 0;
    public GameObject CrabSpeechCam;
    public GameObject PlayerCam;
    public GameObject Canvas;
    public TextMeshProUGUI Speech;


    // Calls the hear speech funnction
    // the player is spawned in this trigger so will automatically run
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(HearSpeech());
        }
    }

    // The king crabs diologue
    // Activates all necesarry objects
    private IEnumerator HearSpeech()
    {
        hasSpoken += 1;

        Speech.text = "My fellow crabs";
        yield return new WaitForSeconds(3f);

        Speech.text = "Have you not heard?";
        yield return new WaitForSeconds(3f);

        Speech.text = "This aquarium will be closing with no plan for where we would stay";
        yield return new WaitForSeconds(3f);

        Speech.text = "Let us find a way to escape, ONCE AND FOR ALL";
        yield return new WaitForSeconds(3f);

        Canvas.gameObject.SetActive(true);
        PlayerCam.gameObject.SetActive(true);
        Canvas.gameObject.SetActive(true);
        CrabSpeechCam.gameObject.SetActive(false);
    }


}

