using UnityEngine;

public class OverrideMusic : MonoBehaviour
{
    private AudioSource overrideSource;

    void Awake()
    {
        overrideSource = GetComponent<AudioSource>();
        if (overrideSource == null)
        {
            Debug.LogError("No AudioSource found on OverrideMusic object!");
        }
    }

    public void Play()
    {
        if (overrideSource != null)
        {
            overrideSource.Play();
        }
    }
}
