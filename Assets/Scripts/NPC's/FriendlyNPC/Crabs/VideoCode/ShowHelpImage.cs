//This script was made using the "PlayWllRunningVideo" script as a reference for guidance on structure/syntax

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowHelpImage : MonoBehaviour
{
    //fundamental variables
    public GameObject HelpPanel;
    public RawImage HelpImage;
    public Texture HelpTexture;
    public TextMeshProUGUI Speech;

    void Start()
    {
        //set initial speech text
        Speech.text = "Struggling? Here's some help!";
        //panel is initially inactive
        HelpPanel.SetActive(false);
    }

    //when  player enters trigger zone, show help image
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HelpPanel.SetActive(true);
            HelpImage.texture = HelpTexture; //assign PNG texture
        }
    }

    //when player exits trigger zone, hide help image
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HelpPanel.SetActive(false);
        }
    }
}
