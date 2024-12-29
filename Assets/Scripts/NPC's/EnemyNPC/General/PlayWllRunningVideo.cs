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

    void Start()
    {
        Speech.text = "Struggling, here's some help";
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WallRunningVideo.gameObject.SetActive(true);
            Video.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WallRunningVideo.gameObject.SetActive(false);
            Video.gameObject.SetActive(false);
        }
    }
}



