using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance = null;

    private void Awake()
    {
        Instance = this;
    }

    public void Shake(float duration, float distance, int speed)
    {
        DOTween.Clear();
        transform.DOShakePosition(duration, distance, speed);
        transform.DOShakeRotation(duration, distance, speed);
    }
}
