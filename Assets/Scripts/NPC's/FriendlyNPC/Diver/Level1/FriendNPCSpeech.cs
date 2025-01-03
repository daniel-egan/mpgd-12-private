using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendNPCSpeech : MonoBehaviour
{
    public TextMeshProUGUI Speech;
    private int hasSpoken = 0;
    public Text Objectives;
    public GameObject OxygenTank;
    public Image OxygenTankIcon;


    public GameObject Player;
    public GameObject MainCamera;
    public GameObject NPCCamera;
    public GameObject Canvas;



    void Start()
    {
        Speech.text = "";
    }

    // When player enters the divers collision zone it sets the player, canvas and main camera as false
    // AND the npc camera to true
    // This mimics a cutscene like effect where the player is soley focused on the diver speaking and what they need
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hasSpoken == 0)
            {
                Player.gameObject.SetActive(false);
                Canvas.gameObject.SetActive(false);

                NPCCamera.gameObject.SetActive(true);
                MainCamera.gameObject.SetActive(false);
                StartCoroutine(ChangeSpeech());

            }
        }
    }

    private void Update()
    {
    }

    // IEnumerator to change the diver's speech as they start talking
    // The objectives chnage upon the end of them talking as well as all the necesary game objects set back to true
    private IEnumerator ChangeSpeech()
    {
        hasSpoken += 1;
        Speech.text = "Hello Friend, I'm in need of some assistance";
        yield return new WaitForSeconds(3f);


        Speech.text = "I have lost my last oxygen tank and this one's about to run out";
        yield return new WaitForSeconds(3f);

        Speech.text = "Please may you help and find it for me? I reckon that pufferfish has something to do with it";
        yield return new WaitForSeconds(3f);

        Objectives.text = "Help find the diver's last oxygen tank";
        yield return new WaitForSeconds(3f);

        Player.gameObject.SetActive(true);
        Canvas.gameObject.SetActive(true);

        MainCamera.gameObject.SetActive(true);
        NPCCamera.gameObject.SetActive(false);
    }
}
