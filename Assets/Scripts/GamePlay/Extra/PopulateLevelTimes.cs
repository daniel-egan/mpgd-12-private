using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class PopulateLevelTimes : MonoBehaviour
{
    public string levelName;

    void Start()
    {
        string topTimesString = PlayerPrefs.GetString($"{levelName}_TIMES", "0,0,0");

        // Sets the text to the string received from storage, or the default string of "0,0,0"
        GetComponent<TextMeshProUGUI>().text = topTimesString;

        TextMeshProUGUI topTime = GetComponent<TextMeshProUGUI>();

        // Checks if the timer is enabled, if it's not then we don't show the times on the level select
        if (!GlobalSettingsManager.Instance.IsTimerOn)
        {
            this.enabled = false;
            topTime.enabled = false;
            return;
        }

    }
}
