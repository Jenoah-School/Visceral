using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class FadeUI : MonoBehaviour
{
    private CanvasGroup canvasGroup = null;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) enabled = false;
    }

    public void FadeOut(float speed)
    {
        canvasGroup.DOFade(0f, speed).SetUpdate(true);
    }

    public void FadeIn(float speed)
    {
        canvasGroup.DOFade(1f, speed).SetUpdate(true);
    }
}
