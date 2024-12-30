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

    void Start()
    {
        // Begin playing music if not already playing
        if (!audioSource.isPlaying)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void Update()
    {
        CheckForOverrideMusic();
    }

    private void CheckForOverrideMusic()
    {
        // Find the first OverrideMusic instance in the current scene
        var overrideMusic = GameObject.FindObjectOfType<OverrideMusic>();
        if (overrideMusic != null)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause(); // Pause the global music manager
            }
        }
        else
        {
            // Resume playback if there is no override in the current scene
            if (!audioSource.isPlaying)
            {
                audioSource.UnPause();
            }
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
