using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioClip mainMenuBGM;
    public AudioClip round1BGM;
    public AudioClip round2BGM;
    public AudioClip round3BGM;
    public AudioClip endGameBGM;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMainMenuBGM();
    }

    public void PlayMainMenuBGM()
    {
        PlayMusic(mainMenuBGM, true);
    }

    public void PlayRound1BGM()
    {
        PlayMusic(round1BGM, false);
    }

    public void PlayRound2BGM()
    {
        PlayMusic(round2BGM, false);
    }

    public void PlayRound3BGM()
    {
        PlayMusic(round3BGM, false);
    }

    public void PlayEndGameBGM()
    {
        PlayMusic(endGameBGM, false);
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    private void PlayMusic(AudioClip clip, bool loop)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Attempted to play a null audio clip.");
        }
    }

    public void FadeOutCurrentBGM(float fadeDuration)
    {
        StartCoroutine(FadeOut(fadeDuration));
    }

    private IEnumerator FadeOut(float fadeDuration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}