using UnityEngine;

public class GlobalSettingsManager : MonoBehaviour
{
    public static GlobalSettingsManager Instance { get; private set; }

    public float MasterVolume { get; private set; }
    public bool IsMusicOn { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSettings();
    }

    public void LoadSettings()
    {
        // Load saved settings or set defaults
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        IsMusicOn = PlayerPrefs.GetInt("MusicToggle", 1) == 1;

        ApplySettings();
    }

    public void ApplySettings()
    {
        // Apply master volume
        AudioListener.volume = MasterVolume;

        // Apply music toggle
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.SetMusicState(IsMusicOn);
        }

        Debug.Log("Settings applied: MasterVolume=" + MasterVolume + ", MusicToggle=" + IsMusicOn);
    }

    public void SaveSettings(float masterVolume, bool isMusicOn)
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetInt("MusicToggle", isMusicOn ? 1 : 0);
        PlayerPrefs.Save();

        LoadSettings(); // Immediately reapply settings after saving
    }
}
