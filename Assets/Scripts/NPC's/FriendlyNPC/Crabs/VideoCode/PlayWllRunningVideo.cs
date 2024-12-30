using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayWllRunningVideo : MonoBehaviour
{
    public GameObject WallRunningVideo;
    public GameObject Video;
    public TextMeshProUGUI Speech;

    // Sets the speech coming out the player
    // Indicates too the player if they are in need of help
    void Start()
    {
        Speech.text = "Struggling, here's some help";
    }

    // When player enters coliision zone the wall running video is played
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WallRunningVideo.gameObject.SetActive(true);
            Video.gameObject.SetActive(true);
        }
    }

    // When the player exits the trigger zone the wall running video is removed
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WallRunningVideo.gameObject.SetActive(false);
            Video.gameObject.SetActive(false);
        }
    }
}



