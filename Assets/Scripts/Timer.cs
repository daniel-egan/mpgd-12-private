using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private Stopwatch _stopwatch = new Stopwatch();

    public bool IsActive()
    {
        return _stopwatch.IsRunning;
    }

    public bool TransitionStopwatch()
    {
        switch (_stopwatch.IsRunning)
        {
            // 2 states: start, stop
            case true:
                _stopwatch.Stop();
                return true;
            case false:
                _stopwatch.Start();
                return true;
        }
    }

    public bool ResetStopwatch()
    {
// Will stop the stopwatch if active
// Will reset back to 0
        _stopwatch.Reset();
        return true;
    }

    public string GetStopwatchTime()
    {
        // Will return a string of the current stopwatch time
        // Should produce "Current Time: 0.000"
        return "Current Time: " + _stopwatch.ToString();
    }

}
