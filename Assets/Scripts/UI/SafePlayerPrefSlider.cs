using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafePlayerPrefSlider : MonoBehaviour
{
    [SerializeField] private string playerPrefsString = "";
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
        float currentSensitivity = PlayerPrefs.GetFloat(playerPrefsString, 1f);
        slider.onValueChanged.AddListener(delegate { SetSensitivity(); });
        slider.value = currentSensitivity;
    }

    void SetSensitivity()
    {
        SaveSensitivity(slider.value);
    }

    void SaveSensitivity(float targetSensitivity)
    {
        PlayerPrefs.SetFloat(playerPrefsString, targetSensitivity);
    }
}
