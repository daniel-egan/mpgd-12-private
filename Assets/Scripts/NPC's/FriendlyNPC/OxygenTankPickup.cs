using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OxygenTankPickup : MonoBehaviour
{
    public TextMeshProUGUI Completion;
    public GameObject Diver;
    public Text Objectives;
    public TextMeshProUGUI Speech;
    public Image OxygenTankIcon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Update the objective text
            Completion.text = "Thank you so much!!! You will be rewarded";
            Speech.text = "";
            Objectives.text = "Return the oxygen tank to the diver";
            BoxCollider boxCollider = Diver.GetComponent<BoxCollider>();
            OxygenTankIcon.gameObject.SetActive(true);
            Destroy(boxCollider);

            // Unlock the achievement
            //CollectableManager.Instance.UnlockCollectable("oxygen_pickup");

            // Destroy the oxygen tank after pickup
            Destroy(gameObject);
        }
    }
}
