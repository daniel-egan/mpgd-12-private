using UnityEngine;

public class ResetProgressButton : MonoBehaviour
{
    // This method will be assigned to the button's OnClick event
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll(); // Deletes all stored PlayerPrefs
        Debug.Log("All progress has been reset.");
    }
}