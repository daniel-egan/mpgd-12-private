using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Timer timer;
    public TextMeshProUGUI completionText;

    // Start is called before the first frame update
    void Start()
    {
        completionText.gameObject.SetActive(false);
        timer.TransitionStopwatch();
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Door")
        {
            timer.TransitionStopwatch();
            completionText.gameObject.SetActive(true);
        }

    }
}
