using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReturnOxygenTank : MonoBehaviour
{

    public Text Objectives;
    public GameObject OxygenTank;
    public Image OxygenTankIcon;

    void Update()
    {
        OxygenTank = GameObject.Find("OxygenTank");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && OxygenTank == null)
        {
            Objectives.text = "";
            OxygenTankIcon.gameObject.SetActive(false);

            // Unlock the achievement
            CollectableManager.Instance.UnlockCollectable("oxygen_pickup");
        }
    }


}
