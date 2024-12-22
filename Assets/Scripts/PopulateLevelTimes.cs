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

        
        GetComponent<TextMeshProUGUI>().text = topTimesString;
        
        }
    }
