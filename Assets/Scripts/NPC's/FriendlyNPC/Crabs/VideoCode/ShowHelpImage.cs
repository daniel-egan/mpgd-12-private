using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowHelpImage : MonoBehaviour
{
    public GameObject HelpPanel; // The panel or object containing the display
    public RawImage HelpImage;   // The RawImage component
    public Texture HelpTexture; // Your PNG texture
    public TextMeshProUGUI Speech;

    void Start()
    {
        // Set the initial speech text
        Speech.text = "Struggling? Here's some help!";
        // Ensure the panel is initially inactive
        HelpPanel.SetActive(false);
    }

    // When the player enters the trigger zone, show the help image
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HelpPanel.SetActive(true);
            HelpImage.texture = HelpTexture; // Assign the PNG texture
        }
    }

    // When the player exits the trigger zone, hide the help image
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HelpPanel.SetActive(false);
        }
    }
}
