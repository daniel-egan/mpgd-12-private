using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SharkProtectedTank : MonoBehaviour
{
    public Text Objectives;
    private int hasSpoken = 0;
    public Image OxygenTankIcon;

    public GameObject FriendSharkCam;
    public GameObject NewOXTankCam;
    public GameObject Player;

    public GameObject MainCamera;

    public TextMeshProUGUI FriendSharkSpeech;

    public GameObject friendShark;
    public GameObject chest;

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameObject.Find("OxygenTank") == null)
        {
            Objectives.text = "";
            OxygenTankIcon.gameObject.SetActive(false);

            if (CollectableManager.Instance != null)
            {
                CollectableManager.Instance.UnlockCollectable("oxygen_pickup");
            }
            else
            {
                Debug.LogWarning("CollectableManager is not initialized.");
            }
            if (hasSpoken == 0)
            {
                StartCoroutine(HandleTankReturnSequence());
            }
  
        }
    }

    private IEnumerator HandleTankReturnSequence()
    {
        hasSpoken += 1;

        MainCamera.gameObject.SetActive(false);
        FriendSharkCam.gameObject.SetActive(true);
        friendShark.gameObject.SetActive(true);
        chest.gameObject.SetActive(true);
        FriendSharkSpeech.gameObject.SetActive(true);

       
        FriendSharkSpeech.text = "Oh, you just wanted the oxygen tank";
        yield return new WaitForSeconds(2f);

        FriendSharkSpeech.text = "I thought you wanted to steal some of our delicious lunch";
        yield return new WaitForSeconds(2f);

        FriendSharkSpeech.text = "How about you jump on my back, I'll help you get to that door";
        yield return new WaitForSeconds(2f);

        FriendSharkSpeech.text = "Don't forget to collect the key!";
        yield return new WaitForSeconds(2f);

 
        FriendSharkSpeech.gameObject.SetActive(false);
        FriendSharkCam.gameObject.SetActive(false);
        NewOXTankCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);





        MainCamera.gameObject.SetActive(true);
        NewOXTankCam.gameObject.SetActive(false);
 
    }
}
