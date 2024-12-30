using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendNPCSpeech3 : MonoBehaviour
{
    public TextMeshProUGUI Speech; 
    private int hasSpoken = 0;
    public Text Objectives;
    public GameObject OxygenTank;
    public Image OxygenTankIcon;


    public GameObject Player;
    public GameObject MainCamera;
    public GameObject NPCCam;
    public GameObject SharkCam;
    public GameObject TankCam;
    public GameObject SharkAttackCam;
    public GameObject Canvas;


    public GameObject SharkAttack;
    public GameObject Livefish;
    public GameObject Skeletonfish;


    void Start()
    {
        Speech.text = "";
    }

    // When player enters the divers collision zone it sets the player, canvas and main camera as false
    // AND the npc camera to true
    // This mimics a cutscene like effect where the player is soley focused on the diver speaking and what they need
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameObject.Find("OxygenTank") != null)
        {
            if (hasSpoken == 0)
            {
                Player.gameObject.SetActive(false);
                Canvas.gameObject.SetActive(false);

                NPCCam.gameObject.SetActive(true);
                MainCamera.gameObject.SetActive(false);
                StartCoroutine(ChangeSpeech3());
                
            }
        }
    }

    private void Update()
    {
    }


    // IEnumerator to change the diver's speech as they start talking
    // The objectives chnage upon the end of them talking as well as all the necesary game objects set back to true
    // During this speech other cameras are set on/ off to show various things in the scene such as the sharks and oxygen tanks
    private IEnumerator ChangeSpeech3()
    {
        hasSpoken += 1;
        Speech.text = "Hello again, you have come so far";
        yield return new WaitForSeconds(3f);

        // Second part of speech
        Speech.text = "I am in need of one more oxygen tank before your journeys end";
        yield return new WaitForSeconds(3f);

        Speech.text = "This tank may be the most difficult to retrieve, guarded by those sharks over there";
        yield return new WaitForSeconds(3f);

        SharkCam.gameObject.SetActive(true);
        NPCCam.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);


        TankCam.gameObject.SetActive(true);
        SharkCam.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);


        NPCCam.gameObject.SetActive(true);
        TankCam.gameObject.SetActive(false);

        Speech.text = "Be wary, the sharks are very DANGEROUS and may attack you";
        yield return new WaitForSeconds(3f);

        SharkAttackCam.gameObject.SetActive(true);
        SharkAttack.gameObject.SetActive(true);
        Livefish.gameObject.SetActive(true);
        NPCCam.gameObject.SetActive(false);
        yield return new WaitForSeconds(3.2f);


        
        Livefish.gameObject.SetActive(false);
        Skeletonfish.gameObject.SetActive(true);
        SharkAttack.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);




        NPCCam.gameObject.SetActive(true);
        SharkAttack.gameObject.SetActive(false);
        SharkAttackCam.gameObject.SetActive(false);


        Speech.text = "Please help me and retrieve it";
        yield return new WaitForSeconds(3f);

        Objectives.text = "Help find the diver's last oxygen tank";
        yield return new WaitForSeconds(3f);

        Player.gameObject.SetActive(true);
        Canvas.gameObject.SetActive(true);

        MainCamera.gameObject.SetActive(true);
        NPCCam.gameObject.SetActive(false);
    }
}
