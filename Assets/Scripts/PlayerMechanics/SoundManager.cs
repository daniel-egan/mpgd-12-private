using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private AudioSource grappleSource;
    private AudioSource jumpSource;
    private AudioSource dashSource;
    private AudioSource superDashSource;

    [Header("Sound Clips")]
    public AudioClip grappleClip;
    public AudioClip jumpClip;
    public AudioClip dashClip;
    public AudioClip superDashClip;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float soundEffectVolume = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        grappleSource = gameObject.AddComponent<AudioSource>();
        jumpSource = gameObject.AddComponent<AudioSource>();
        dashSource = gameObject.AddComponent<AudioSource>();
        superDashSource = gameObject.AddComponent<AudioSource>();

        if (grappleClip) grappleSource.clip = grappleClip;
        if (jumpClip) jumpSource.clip = jumpClip;
        if (dashClip) dashSource.clip = dashClip;
        if (superDashClip) superDashSource.clip = superDashClip;

        grappleSource.playOnAwake = false;
        jumpSource.playOnAwake = false;
        dashSource.playOnAwake = false;
        superDashSource.playOnAwake = false;
    }

    public void PlayGrapple()
    {
        if (grappleClip != null)
        {
            grappleSource.volume = soundEffectVolume;
            grappleSource.PlayOneShot(grappleClip);
        }
    }

    public void PlayJump()
    {
        if (jumpClip != null)
        {
            jumpSource.volume = soundEffectVolume;
            jumpSource.PlayOneShot(jumpClip);
        }
    }

    public void PlayDash()
    {
        if (dashClip != null)
        {
            dashSource.volume = soundEffectVolume;
            dashSource.PlayOneShot(dashClip);
        }
    }

    public void PlaySuperDash()
    {
        if (superDashClip != null)
        {
            superDashSource.volume = soundEffectVolume;
            superDashSource.PlayOneShot(superDashClip);
        }
    }
}
