using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource movementSource;
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mainAudioMixer;

    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip axeSlash;
    public AudioClip axeHit;
    public AudioClip waterSpray;
    public AudioClip firePutOut;

    [Header("Movement Audio Clip")]
    public AudioClip walk;
    public AudioClip run;
    public AudioClip jump;
    public AudioClip jumpLand;
    void Awake()
    {
        // Set up singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: keep audio across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Start background music
        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip audioClip)
    {
        SFXSource.PlayOneShot(audioClip);
    }

    public void PlaySFXLoop(AudioClip audioClip)
    {
        if (SFXSource.clip == audioClip && SFXSource.isPlaying)
            return; // Don't restart if already playing the same clip

        SFXSource.clip = audioClip;
        SFXSource.loop = true;
        SFXSource.Play();
    }
    public void StopSFXLoop()
    {
        SFXSource.loop = false;
        SFXSource.Stop();
    }

    public void PlayMovementLoop(AudioClip audioClip)
    {
        if (movementSource.clip == audioClip && movementSource.isPlaying)
            return; // Don't restart if already playing the same clip

        movementSource.clip = audioClip;
        movementSource.loop = true;
        movementSource.Play();
    }

    public void StopMovementLoop()
    {
        movementSource.loop = false;
        movementSource.Stop();
    }

}