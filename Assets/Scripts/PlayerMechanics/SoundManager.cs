using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private AudioSource grappleSource;
    private AudioSource jumpSource;
    private AudioSource dashSource;
    private AudioSource superDashBuildupSource;
    private AudioSource superDashLossSource;
    private AudioSource superDashUnlockSource;
    private AudioSource superDashSource;
    public AudioSource wallRunningSource;
    private AudioSource resetSource;

    [Header("Sound Clips")]
    public AudioClip grappleClip;
    public AudioClip jumpClip;
    public AudioClip dashClip;
    public AudioClip superDashBuildupClip;
    public AudioClip superDashLossClip;
    public AudioClip superDashUnlockClip;
    public AudioClip superDashClip;
    public AudioClip wallRunningClip;
    public AudioClip resetClip;

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
        superDashBuildupSource = gameObject.AddComponent<AudioSource>();
        superDashLossSource = gameObject.AddComponent<AudioSource>();
        superDashUnlockSource = gameObject.AddComponent<AudioSource>();
        superDashSource = gameObject.AddComponent<AudioSource>();
        wallRunningSource = gameObject.AddComponent<AudioSource>();
        resetSource = gameObject.AddComponent<AudioSource>();

        if (grappleClip) grappleSource.clip = grappleClip;
        if (jumpClip) jumpSource.clip = jumpClip;
        if (dashClip) dashSource.clip = dashClip;
        if (superDashBuildupClip) superDashBuildupSource.clip = superDashBuildupClip;
        if (superDashLossClip) superDashLossSource.clip = superDashLossClip;
        if (superDashUnlockClip) superDashUnlockSource.clip = superDashUnlockClip;
        if (superDashClip) superDashSource.clip = superDashClip;
        if (wallRunningClip) wallRunningSource.clip = wallRunningClip;
        if (resetClip) resetSource.clip = resetClip;

        grappleSource.playOnAwake = false;
        jumpSource.playOnAwake = false;
        dashSource.playOnAwake = false;
        superDashBuildupSource.playOnAwake = false;
        superDashLossSource.playOnAwake = false;
        superDashUnlockSource.playOnAwake = false;
        superDashSource.playOnAwake = false;
        wallRunningSource.playOnAwake = false;
        resetSource.playOnAwake = false;
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

    public void PlaySDBuildup()
    {
        if (superDashBuildupClip != null)
        {
            superDashBuildupSource.volume = soundEffectVolume;
            superDashBuildupSource.PlayOneShot(superDashBuildupClip);
        }
    }

    public void PlaySDLoss()
    {
        if (superDashLossClip != null)
        {
            superDashLossSource.volume = soundEffectVolume;
            superDashLossSource.PlayOneShot(superDashLossClip);
        }
    }

    public void PlaySDUnlock()
    {
        if (superDashUnlockClip != null)
        {
            superDashUnlockSource.volume = soundEffectVolume;
            superDashUnlockSource.PlayOneShot(superDashUnlockClip);
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

    public void PlayWallrunning()
    {
        if (wallRunningClip != null)
        {
            wallRunningSource.volume = soundEffectVolume;
            wallRunningSource.PlayOneShot(wallRunningClip);
            
            if (wallRunningSource.isPlaying)
            {
                return;
            }
        }
    }

    public void PlayReset()
    {
        if (resetClip != null)
        {
            resetSource.volume = soundEffectVolume;
            resetSource.PlayOneShot(resetClip);
        }
    }
}
