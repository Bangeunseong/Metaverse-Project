using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    private AudioSource audioSource;

    public void Play(AudioClip audioClip, float soundEffectVolume, float soundEffectPitchVariance)
    {
        if (audioSource == null) audioSource = Helper.GetComponent_Helper<AudioSource>(gameObject);

        CancelInvoke();
        audioSource.clip = audioClip;
        audioSource.volume = soundEffectVolume;
        audioSource.Play();
        audioSource.pitch = 1f + Random.Range(-soundEffectPitchVariance, soundEffectPitchVariance);

        Invoke("Disable", audioClip.length + 2);
    }

    public void Disable()
    {
        audioSource.Stop();
        Destroy(gameObject);
    }
}
