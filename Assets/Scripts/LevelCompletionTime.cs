using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletionTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        
        // Get the most recent time
        var levelCompletionTime = PlayerPrefs.GetString("LevelCompletionTime");
        print(levelCompletionTime);
        // Get the most recent level completed
        var levelCompletionName = PlayerPrefs.GetString("LevelCompletionName");
        print(levelCompletionName);
     
        TextMeshProUGUI timeText = GameObject.Find("Time").GetComponent<TextMeshProUGUI>();
        timeText.text = $"{levelCompletionName} completed in {levelCompletionTime}";

        // Check against top 3 times for this level
        
    }
}
