using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    // Function to load a scene by name
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Optional: Function to quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
