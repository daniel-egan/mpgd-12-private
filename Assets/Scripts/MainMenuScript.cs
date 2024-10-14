using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // This method will be called when the Play button is clicked
    public void PlayGame()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
