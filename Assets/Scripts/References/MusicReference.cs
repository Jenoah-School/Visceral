using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicReference : MonoBehaviour
{
    public static MusicReference instance = null;
    [SerializeField] private List<AudioSource> audioSources = new List<AudioSource>();
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float targetVolume = 0.25f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void ListenTo(int audioSourceIndex)
    {
        if (audioSourceIndex >= audioSources.Count) return;
        if(audioSourceIndex == -1)
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                audioSources[i].DOFade(0f, fadeSpeed);
            }
        }
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (i == audioSourceIndex)
            {
                if (!audioSources[i].isPlaying) audioSources[i].Play();
                audioSources[i].DOFade(targetVolume, fadeSpeed);
            }
            else
            {
                audioSources[i].DOFade(0f, fadeSpeed);
            }
        }
    }
}
