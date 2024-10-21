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
        completionText.gameObject.SetActive(false);
        hasKeyText.gameObject.SetActive(false);
        timer.TransitionStopwatch();
        hasKey = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Door")
        {
            if (hasKey)
            {
                timer.TransitionStopwatch();
                completionText.gameObject.SetActive(true);
                SceneManager.LoadScene("Completion");
            }
            else
            {
                hasKeyText.gameObject.SetActive(true);
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
