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
        float[] updatedTimes = CompareAndUpdateTop3Times(times, levelCompletionTime);

        SaveTop3Times(updatedTimes);
    }

    private void SaveTop3Times(float[] updatedTimes)
    {
        throw new NotImplementedException();
    }

    private float[] CompareAndUpdateTop3Times(float[] times, float levelCompletionTime)
    {
        float[] newArray = new float[times.Length];
        bool valueChange = false;
        
        for (var i = 0; i < 3; i++)
        {
            // Check if the levelCompletionTime is quicker than the time at the current index
            // Or if there is no completion time (0) at this current index
            if (times[i] == 0 || times[i] > levelCompletionTime)
            {
                // And then we can just insert at this current index and copy 
                print($"Index {i}");
                print($"Replacing {times[i]}");
				
                Array.Copy(times, 0, newArray, 0, i);
                newArray[i] = levelCompletionTime;
                
                Array.Copy(times, i, newArray, i + 1, 3-i-1);
				
                valueChange = true;
                break;
            }
            // Otherwise it has been completed and is slower than the current time so we move on
        }

        // If no times are slower than we just return the original array
        return valueChange == false ? times : newArray;
    }

    private float[] GetTop3Times(string levelCompletionName)
    {
        string topTimesString = PlayerPrefs.GetString($"{levelCompletionName}_TIMES", "0,0,0");
        float[] times = topTimesString.Split(',').Select(float.Parse).ToArray();
        return times;
    }
}
