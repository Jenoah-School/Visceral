using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AlphaOverTime : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float intensity = 1f;
    [SerializeField] private AnimationCurve alphaCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));
    [SerializeField] private Color startColor = new Color(1f, 1f, 1f, 0.5f);
    [SerializeField] private bool canFade = true;

    private Image image;
    private float startAlpha;
    private float currentTime = 0f;
    private Color targetColor;

    // Start is called before the first frame update
    void Start()
    {
        if (!TryGetComponent(out image))
        {
            enabled = false;
        }

        startAlpha = startColor.a;
        targetColor = startColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canFade) return;

        currentTime += Time.deltaTime;

        float normalizedTime = (currentTime * speed) % 1f;

        targetColor.a = Mathf.Clamp(startAlpha + 1f * (alphaCurve.Evaluate(normalizedTime) * intensity), 0f, 255f);
        image.color = targetColor;
    }

    public void FadeOut(float speed)
    {
        canFade = false;
        image.DOFade(0f, speed);
    }

    public void FadeIn(float speed)
    {
        image.DOFade(targetColor.a, speed);
        StopAllCoroutines();
        StartCoroutine(SetFadeState(true, speed));
    }

    IEnumerator SetFadeState(bool fadeState, float delay)
    {
        yield return new WaitForSeconds(delay);
        canFade = fadeState;
    }
}
