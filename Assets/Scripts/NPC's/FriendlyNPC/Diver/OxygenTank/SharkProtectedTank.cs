using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SharkProtectedTank : MonoBehaviour
{
    public Text Objectives;
    public Image OxygenTankIcon;

    public GameObject TankCam;
    public GameObject FriendSharkCam;
    public GameObject NewOXTankCam;
    public GameObject Player;

    public TextMeshProUGUI FriendSharkSpeech;

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameObject.Find("OxygenTank") == null)
        {
            StartCoroutine(HandleTankReturnSequence());
        }
    }

    private IEnumerator HandleTankReturnSequence()
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

        Player.gameObject.SetActive(false);
        TankCam.gameObject.SetActive(false);
        FriendSharkCam.gameObject.SetActive(true);
        FriendSharkSpeech.text = "Oh, you just wanted the oxygen tank";
        yield return new WaitForSeconds(2f);

        Objectives.text = "I thought you wanted to steal some of our delicious lunch";
        yield return new WaitForSeconds(2f);

        Objectives.text = "How about you jump on my back, I'll help you get to that door";
        yield return new WaitForSeconds(2f);

        Objectives.text = "Don't forget to collect the key!";
        yield return new WaitForSeconds(2f);

        NewOXTankCam.gameObject.SetActive(true);
        FriendSharkCam.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);

        Player.gameObject.SetActive(true);
        NewOXTankCam.gameObject.SetActive(false);
        TankCam.gameObject.SetActive(true);
    }
}
