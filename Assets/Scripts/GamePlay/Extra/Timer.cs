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

    public float GetStopwatchTime()
    {
        // Will return a string of the current stopwatch time
        // Should produce "123.123"
        return float.Parse($"{_stopwatch.Elapsed.TotalSeconds:00}.{_stopwatch.Elapsed.Milliseconds:000}");
    }

    void Start()
    {
        // This gets the UI text that corresponds to the "Current Time"
        _stopwatchText = GetComponent<TextMeshProUGUI>();

        if (!GlobalSettingsManager.Instance.IsTimerOn)
        {
            this.enabled = false;
            _stopwatchText.enabled = false;
            return;
        }
    }

    private void Update()
    {
        _stopwatchText.text = GetStopwatchTime().ToString();
    }
}
