using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeFader : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup masterGroup = null;
    [SerializeField] private string mixerName = "MasterVolume";
    [SerializeField] private float fadeTime = 1f;
    private float targetLevel = 0f;

    void Start()
    {
        targetLevel = PlayerPrefs.GetFloat(mixerName, 0f);
        masterGroup.audioMixer.SetFloat(mixerName, -80f);
        FadeIn();
    }

    public void FadeIn()
    {
        StartCoroutine(FadeToTarget(targetLevel));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeToTarget(-80f));
    }

    public void FadeToVolumeLevel(float volumeLevel)
    {
        StopAllCoroutines();
        if (volumeLevel > targetLevel)
        {
            volumeLevel = targetLevel;
        }
        StartCoroutine(FadeToTarget(volumeLevel));
    }

    private IEnumerator FadeToTarget(float targetVolume)
    {
        masterGroup.audioMixer.GetFloat(mixerName, out float currentVolume);
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float newVolume = Mathf.SmoothStep(currentVolume, targetVolume, elapsedTime / fadeTime);
            masterGroup.audioMixer.SetFloat(mixerName, newVolume);
            yield return new WaitForEndOfFrame();
        }

        masterGroup.audioMixer.SetFloat(mixerName, targetVolume);
    }
}
