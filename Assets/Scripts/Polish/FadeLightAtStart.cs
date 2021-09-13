using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeLightAtStart : MonoBehaviour
{
    private Light lightSource = null;
    [SerializeField] private float targetValue = 0f;
    [SerializeField] private float fadeSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        lightSource = GetComponent<Light>();
        if (lightSource == null) enabled = false;
        lightSource.DOIntensity(targetValue, fadeSpeed);
    }
}
