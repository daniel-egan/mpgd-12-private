using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Timer timer;

    // Start is called before the first frame update
    void Start()
    {
        timer.TransitionStopwatch();
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Door")
        {
            timer.TransitionStopwatch();
        }

    }
}
