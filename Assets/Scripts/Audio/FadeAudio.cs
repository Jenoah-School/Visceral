using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeAudio : MonoBehaviour
{
    private MusicReference musicReference = null;

    private void Start()
    {
        musicReference = MusicReference.instance;
    }

   public void FadeTo(int audioSourceId)
    {
        musicReference.ListenTo(audioSourceId);
    }
}
