using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField][Range(0f, 1f)] private float soundEffectVolume;
    [SerializeField][Range(0f, 1f)] private float soundEffectPicthVariance;
    [SerializeField][Range(0f, 1f)] private float musicVolume;
    [SerializeField] private SoundSource soundSourcePrefab;
    
    private AudioSource musicAudioSource;

    // Singleton
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
                instance = Helper.GetComponent_Helper<SoundManager>(GameObject.FindWithTag(nameof(SoundManager)));
            return instance;
        }
    }

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);

        musicAudioSource = Helper.GetComponent_Helper<AudioSource>(gameObject);
        musicAudioSource.volume = musicVolume;
        musicAudioSource.loop = true;
    }

    /// <summary>
    /// Changes background music clip
    /// </summary>
    /// <param name="clip"></param>
    public void ChangeBackgroundMusic(AudioClip clip)
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }

    /// <summary>
    /// Plays selected clip
    /// </summary>
    /// <param name="clip"></param>
    public static void PlayClip(AudioClip clip)
    {
        SoundSource obj = Instantiate(Instance.soundSourcePrefab);
        SoundSource soundSource = Helper.GetComponent_Helper<SoundSource>(obj.gameObject);
        soundSource.Play(clip, Instance.soundEffectVolume, Instance.soundEffectPicthVariance);
    }
}
