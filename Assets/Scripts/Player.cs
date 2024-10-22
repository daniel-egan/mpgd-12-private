using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Timer timer;
    public TextMeshProUGUI completionText;
    public bool hasKey;
    public TextMeshProUGUI hasKeyText;

    // Start is called before the first frame update
    void Start()
    {
        // This should remove the UI texts from showing to player
        completionText.gameObject.SetActive(false);
        hasKeyText.gameObject.SetActive(false);

        // Starts the stopwatch, counting upwards
        timer.TransitionStopwatch();

        // Used when checking if the door can be accessed
        hasKey = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Door")
        {
            if (hasKey)
            {
                // Stops the timer
                timer.TransitionStopwatch();
                // Shows the level completed text
                completionText.gameObject.SetActive(true);
                SceneManager.LoadScene("Completion");
            }
            else
            {
                hasKeyText.gameObject.SetActive(true);
                // This means that the player does not have the key
                // So this will show the text for 2 seconds and will then
                // remove it from the display
                // Adapted from https://stackoverflow.com/questions/25671746/coroutines-unity
                StartCoroutine(HasKeyTextDelay());
                IEnumerator HasKeyTextDelay()
                {
                    yield return new WaitForSeconds(2);
                    hasKeyText.gameObject.SetActive(false);
                }
            }
        }
    }
}
