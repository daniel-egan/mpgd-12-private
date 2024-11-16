using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        
    float[] GetTop3Times(string levelCompletionName)
    {
        string topTimesString = PlayerPrefs.GetString($"{levelCompletionName}_TIMES", "0,0,0");
        float[] times = topTimesString.Split(',').Select(float.Parse).ToArray();
        return times;
    }
}
