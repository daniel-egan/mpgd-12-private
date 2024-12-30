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


    // When the player is within the trigger zone of the oxygen tank the objectives are updated/
    // and the oxygen tank is destroyed
    // Also the oxygen tank icon in the inventory is displayed
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

            // Destroy the oxygen tank after pickup
            Destroy(gameObject);
        }
    }
}
