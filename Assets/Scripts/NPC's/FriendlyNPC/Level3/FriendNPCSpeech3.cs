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


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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

    private IEnumerator ChangeSpeech3()
    {
        hasSpoken += 1;
        Speech.text = "Hello again, you have come so far";
        yield return new WaitForSeconds(3f);

        // Second part of speech
        Speech.text = "I am in need of one more oxygen tank before your journeys end";
        yield return new WaitForSeconds(3f);

        Speech.text = "This tank may be the most difficult to retrieve, guarded by those sharks over their";
        yield return new WaitForSeconds(3f);

        SharkCam.gameObject.SetActive(true);
        NPCCam.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);


        TankCam.gameObject.SetActive(true);
        SharkCam.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);


        NPCCam.gameObject.SetActive(true);
        TankCam.gameObject.SetActive(false);

        Speech.text = "Be wary, the sharks may attack you if you give them a reason";
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
