using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Toggle musicToggle;

    void Start()
    {
        // Initialize sliders and toggles from the GlobalSettingsManager
        masterVolumeSlider.value = GlobalSettingsManager.Instance.MasterVolume;
        musicToggle.isOn = GlobalSettingsManager.Instance.IsMusicOn;
    }

    public void SaveSettings()
    {
        GlobalSettingsManager.Instance.SaveSettings(masterVolumeSlider.value, musicToggle.isOn);
        Debug.Log("Settings Saved and Applied!");
    }

    public void ApplySettings()
    {
        GlobalSettingsManager.Instance.ApplySettings();
    }
}
