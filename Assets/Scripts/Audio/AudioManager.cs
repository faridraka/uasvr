using UnityEngine;

// Singleton buat mainin semua SFX. Drag semua AudioClip di Inspector.
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource sfxSource;
    public AudioSource engineLoopSource;

    [Header("Clips")]
    public AudioClip clickClip;
    public AudioClip engineStartClip;
    public AudioClip engineStopClip;
    public AudioClip leverClip;
    public AudioClip warningBeepClip;
    public AudioClip successChimeClip;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void OnEnable()
    {
        if (TrainingManager.Instance != null)
        {
            TrainingManager.Instance.OnMistake += PlayWarningBeep;
            TrainingManager.Instance.OnTrainingComplete += PlaySuccessChime;
        }
    }

    void OnDisable()
    {
        if (TrainingManager.Instance == null) return;
        TrainingManager.Instance.OnMistake -= PlayWarningBeep;
        TrainingManager.Instance.OnTrainingComplete -= PlaySuccessChime;
    }

    public void PlayClick() { if (clickClip != null) sfxSource.PlayOneShot(clickClip); }
    public void PlayLever() { if (leverClip != null) sfxSource.PlayOneShot(leverClip); }
    public void PlayWarningBeep() { if (warningBeepClip != null) sfxSource.PlayOneShot(warningBeepClip); }
    public void PlaySuccessChime() { if (successChimeClip != null) sfxSource.PlayOneShot(successChimeClip); }

    public void PlayEngineStart()
    {
        if (engineStartClip != null) sfxSource.PlayOneShot(engineStartClip);
        if (engineLoopSource != null) engineLoopSource.Play();
    }

    public void PlayEngineStop()
    {
        if (engineStopClip != null) sfxSource.PlayOneShot(engineStopClip);
        if (engineLoopSource != null) engineLoopSource.Stop();
    }
}
