using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OxygenTankPickup : MonoBehaviour
{
    public TextMeshProUGUI Completion;
    public GameObject Diver;
    public TextMeshProUGUI Objectives;
    public TextMeshProUGUI Speech;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Update the objective text
            Completion.text = "Thank you so much!!! You will be rewarded";
            Speech.text = "";
            Objectives.text = "Return the oxygen tank to the diver";
            BoxCollider boxCollider = Diver.GetComponent<BoxCollider>();
            Destroy(boxCollider);

            // Destroy the oxygen tank after pickup
            Destroy(gameObject);
        }
    }
}
