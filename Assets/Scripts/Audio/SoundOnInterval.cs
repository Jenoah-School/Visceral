using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundOnInterval : MonoBehaviour
{
    [SerializeField] private float intervalTime = 5f;
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();

    [Header("Random")]
    [SerializeField] private bool randomInterval = true;
    [SerializeField] private float minimalIntvervalTime = 3f;
    [SerializeField] private float maximumIntervalTime = 15f;

    private float nextSoundTime = 0f;
    private AudioSource audioSource = null;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SetNextInterval();
    }

    private void Update()
    {
        if(Time.time >= nextSoundTime)
        {
            SetNextInterval();
            AudioClip audioClip = GetRandomAudioClip();
            if (audioClip != null) audioSource.PlayOneShot(audioClip);
        }
    }

    private void SetNextInterval()
    {
        if (randomInterval) intervalTime = Random.Range(minimalIntvervalTime, maximumIntervalTime);
        nextSoundTime = Time.time + intervalTime;
    }

    private AudioClip GetRandomAudioClip()
    {
        if(audioClips.Count >= 0)
        {
            return audioClips[Random.Range(0, audioClips.Count)];
        }
        else
        {
            return null;
        }
    }
}
