using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeLightAtStart : MonoBehaviour
{
    private Light light = null;
    [SerializeField] private float targetValue = 0f;
    [SerializeField] private float fadeSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        if (light == null) enabled = false;
        light.DOIntensity(targetValue, fadeSpeed);
    }
}
