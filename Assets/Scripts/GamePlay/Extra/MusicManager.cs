using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; } // Singleton instance
    private AudioSource audioSource; // The AudioSource for the music

    void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Get or add the AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on MusicManager!");
        }
    }

    public void SetMusicState(bool isMusicOn)
    {
        if (audioSource != null)
        {
            audioSource.mute = !isMusicOn;
        }
    }

    public bool GetMusicState()
    {
        return audioSource != null && !audioSource.mute;
    }
}
