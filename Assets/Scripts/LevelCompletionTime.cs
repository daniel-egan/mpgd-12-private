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
        var levelCompletionTime = PlayerPrefs.GetFloat("LevelCompletionTime");
        print(levelCompletionTime);
        
        // Get the most recent level completed
        var levelCompletionName = PlayerPrefs.GetString("LevelCompletionName");
        print(levelCompletionName);
     
        TextMeshProUGUI timeText = GameObject.Find("Time").GetComponent<TextMeshProUGUI>();
        timeText.text = $"{levelCompletionName} completed in {levelCompletionTime}";

        // Get the 3 best times for the level
        float[] times = GetTop3Times(levelCompletionName);
        
        // Compare the top 3 to see if completion time was quicker
        CompareandUpdateTop3Times(times, levelCompletionTime);
    }

    private void CompareandUpdateTop3Times(float[] times, float levelCompletionTime)
    {
        for (var i = 0; i < 3; i++)
        {
            if (times[i] == 0)
            {
                // Then we know that the player has not completed the level at this index
                // And then we can just insert at this current index
            }
            
            // Check if the levelCompletionTime is quicker than the time at the current index
            if (times[i] > levelCompletionTime)
            {
                
            }
            
            // Otherwise it has been completed and is slower than the current time so we move on
        }
    }

    private float[] GetTop3Times(string levelCompletionName)
    {
        string topTimesString = PlayerPrefs.GetString($"{levelCompletionName}_TIMES", "0,0,0");
        float[] times = topTimesString.Split(',').Select(float.Parse).ToArray();
        return times;
    }
}
