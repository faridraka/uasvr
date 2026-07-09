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

    private bool engineLoopPlaying = false;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Update()
    {
        if (TrainingManager.Instance == null) return;

        if (TrainingManager.Instance.isEngineOn)
            PlayEngineStart();
        else
            PlayEngineStop();
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

    public void PlayClick() { PlayOneShot(clickClip); }
    public void PlayLever() { PlayOneShot(leverClip); }
    public void PlayWarningBeep() { PlayOneShot(warningBeepClip); }
    public void PlaySuccessChime() { PlayOneShot(successChimeClip); }

    public void PlayEngineStart()
    {
        if (engineLoopSource == null || engineStartClip == null)
            return;

        if (engineLoopPlaying && engineLoopSource.isPlaying)
            return;

        engineLoopSource.clip = engineStartClip;
        engineLoopSource.loop = true;
        engineLoopSource.Play();
        engineLoopPlaying = true;
    }

    public void PlayEngineStop()
    {
        if (engineLoopSource == null)
        {
            engineLoopPlaying = false;
            return;
        }

        if (engineLoopSource.isPlaying)
            engineLoopSource.Stop();

        engineLoopPlaying = false;
    }

    private void PlayOneShot(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip);
    }
}
