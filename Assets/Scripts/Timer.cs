using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private Stopwatch _stopwatch = new Stopwatch();
    private TextMeshProUGUI _stopwatchText;
    
    public bool IsActive()
    {
        return _stopwatch.IsRunning;
    }

    public void TransitionStopwatch()
    {
        switch (_stopwatch.IsRunning)
        {
            // 2 states: start, stop
            case true:
                _stopwatch.Stop();
                return;
            case false:
                _stopwatch.Start();
                return;
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
        return String.Format("Current Time: {0:00}s {1:000}ms", _stopwatch.Elapsed.TotalSeconds, _stopwatch.Elapsed.Milliseconds);
    }

    void Start()
    {
        _stopwatchText = GetComponent<TextMeshProUGUI>();
        TransitionStopwatch();
    }

    private void Update()
    {
        _stopwatchText.text = GetStopwatchTime();
    }
}
