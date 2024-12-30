using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReturnOxygenTank : MonoBehaviour
{

    public Text Objectives;
    public Image OxygenTankIcon;

    void Update()
    {
        
    }
    // When the player is in the trigger zone of the diver to return the oxygen tank the objectives are removed,
    // The oxygen tank game icon is set to false,
    // and the player has earned themselves an achievement
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
        }

    }


}
