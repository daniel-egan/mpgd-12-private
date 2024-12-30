using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendNPCSpeech2 : MonoBehaviour
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

    public GameObject WallsCam;
    public GameObject TankCamera;



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
        Speech.text = "Hello Friend, I'm in need of even more assistance";
        yield return new WaitForSeconds(3f);


        Speech.text = "Another one of my oxygen tanks have gone missing";
        yield return new WaitForSeconds(3f);

        NPCCamera.gameObject.SetActive(false);
        TankCamera.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);

        NPCCamera.gameObject.SetActive(true);
        TankCamera.gameObject.SetActive(false);
        Speech.text = "That squid seems very suspicious over there next to those very weak walls";
        yield return new WaitForSeconds(3f);

        WallsCam.gameObject.SetActive(true);
        NPCCamera.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);


        WallsCam.gameObject.SetActive(false);
        NPCCamera.gameObject.SetActive(true);
        Objectives.text = "Help find the diver's last oxygen tank";
        yield return new WaitForSeconds(3f);

        Player.gameObject.SetActive(true);
        Canvas.gameObject.SetActive(true);

        MainCamera.gameObject.SetActive(true);
        NPCCamera.gameObject.SetActive(false);
    }
}
