using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ReturnOxygenTank : MonoBehaviour
{

    public Text Objectives;
    public Image OxygenTankIcon;

    void Update()
    {
        
    }

    private IEnumerator UnlockAfterDelay()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(4);

        // Unlock the "all_tank" collectable
        CollectableManager.Instance.UnlockCollectable("all_tank");
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
            if (CollectableManager.Instance != null && SceneManager.GetActiveScene().name == "Level2")
            {
                CollectableManager.Instance.UnlockCollectable("oxygen_pickup1");
                if (CollectableManager.Instance.AreAllTanksCollected())
                {
                    StartCoroutine(UnlockAfterDelay());
                }
            }
            else if (CollectableManager.Instance != null && SceneManager.GetActiveScene().name == "Level3")
            {
                CollectableManager.Instance.UnlockCollectable("oxygen_pickup2");
                if (CollectableManager.Instance.AreAllTanksCollected())
                {
                    StartCoroutine(UnlockAfterDelay());
                }
            }
            else
            {
                Debug.LogWarning("CollectableManager is not initialized.");
            }
        }

    }


}
