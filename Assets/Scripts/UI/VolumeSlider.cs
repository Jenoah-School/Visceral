using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixGroup = null;
    [SerializeField] private string volumeParameterName = "";
    private Slider slider;

    /// <summary>
    /// Initiates and if existing, loads volume from memory. Also updates UI accordingly
    /// </summary>
    void Awake()
    {
        slider = GetComponent<Slider>();
        float currentVolume = PlayerPrefs.GetFloat(volumeParameterName, 1f);
        slider.onValueChanged.AddListener(delegate { OnVolumeSliderChange(); });
        SetVolume(currentVolume);
        slider.value = currentVolume;
    }

    /// <summary>
    /// Sets volume to slider value
    /// </summary>
    void OnVolumeSliderChange()
    {
        SetVolume(slider.value);
    }

    /// <summary>
    /// Sets and saves volume to given target volume
    /// </summary>
    /// <param name="targetVolume"></param>
    void SetVolume(float targetVolume)
    {
        float mixerTargetVolume = Mathf.Log10(targetVolume) * 20f;
        mixGroup.audioMixer.SetFloat(volumeParameterName, mixerTargetVolume);
        PlayerPrefs.SetFloat(volumeParameterName, targetVolume);
    }
}
