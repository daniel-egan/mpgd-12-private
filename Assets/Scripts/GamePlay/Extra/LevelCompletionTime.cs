using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletionTime : MonoBehaviour
{
    // This sets the times that will be used in order to see if the player is quicker than our developer times
    private readonly Dictionary<string, float> _achievementTimes = new Dictionary<string, float>
    {
        {"Level1", 90f},
        {"Level2", 300f},
        {"Level3", 300f},
        {"BonusLevel", 300f},
    };

    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI timeText = GameObject.Find("Time").GetComponent<TextMeshProUGUI>();

        if (!GlobalSettingsManager.Instance.IsTimerOn)
        {
            this.enabled = false;
            timeText.enabled = false;
            return;
        }

        // Get the most recent time
        var levelCompletionTime = PlayerPrefs.GetFloat("LevelCompletionTime");
        print(levelCompletionTime);

        // Get the most recent level completed
        var levelCompletionName = PlayerPrefs.GetString("LevelCompletionName");
        print(levelCompletionName);

        timeText.text = $"{levelCompletionName} completed in {levelCompletionTime}";

        // Check if level has been beaten quicker than achievement time
        CheckAchievementTimes(levelCompletionTime, levelCompletionName);

        // Get the 3 best times for the level
        float[] times = GetTop3Times(levelCompletionName);

        // Compare the top 3 to see if completion time was quicker
        float[] updatedTimes = CompareAndUpdateTop3Times(times, levelCompletionTime);

        print($"TIMES {string.Join(", ", updatedTimes)}");
        SaveTop3Times(updatedTimes, levelCompletionName);
    }

    private void CheckAchievementTimes(float levelCompletionTime, string levelCompletionName)
    {
        // Compares the time the player just did, to the stored time in the dictionary (developer times)
        if (levelCompletionTime < _achievementTimes[levelCompletionName])
        {
            // Calls into the singleton to unlock the achievement (stored originally in "achievements.json")
            CollectableManager.Instance.UnlockCollectable($"time_{levelCompletionName}");
            print($"Unlocked {levelCompletionName}");
        }
    }

    private void SaveTop3Times(float[] updatedTimes, string levelCompletionName)
    {
        // Saves a string of the 3 stop times into storage so that across loads it is saved
        PlayerPrefs.SetString($"{levelCompletionName}_TIMES", string.Join(",", updatedTimes));
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

                // Below copies the values quicker than the time that has just been completed
                // Copies ith number of values from index 0 in "times" array into index 0 of "newArray"
                Array.Copy(times, 0, newArray, 0, i);
                // This performs the insertion of the new time into the correct place
                newArray[i] = levelCompletionTime;

                // This copies the values that were slower than the time just completed
                // Performs a similar copy from "times" to "newArray" but taking into account the insertion
                Array.Copy(times, i, newArray, i + 1, 3 - i - 1);

                // Changes the variable to make sure that we return the correct array (the new updated times or the old ones)
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
        // Splitting on a "," character
        // Converts into floats
        // Puts these new floats into an array
        float[] times = topTimesString.Split(',').Select(float.Parse).ToArray();
        return times;
    }
}
