using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeText : MonoBehaviour
{
    [SerializeField] private Color targetColor = Color.white;

    private TMPro.TextMeshProUGUI textObject;
    private Color defaultColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        textObject = GetComponent<TMPro.TextMeshProUGUI>();
        if (textObject == null) enabled = false;
        defaultColor = textObject.color;
    }

    public void FadeIn(float fadeSpeed)
    {
        StopAllCoroutines();
        StartCoroutine(FadeColor(1f, fadeSpeed));
    }

    public void FadeToTarget(float fadeSpeed)
    {
        textObject.DOColor(targetColor, fadeSpeed).SetUpdate(true);
    }
    
    public void FadeToDefault(float fadeSpeed)
    {
        textObject.DOColor(defaultColor, fadeSpeed).SetUpdate(true);
    }

    public void FadeOut(float fadeSpeed)
    {
        StopAllCoroutines();
        StartCoroutine(FadeColor(0f, fadeSpeed));
    }
    
    IEnumerator FadeColor(float targetAlpha, float fadeSpeed)
    {
        float currentAlpha = textObject.alpha;
        float elapsedTime = 0f;

        while(elapsedTime < fadeSpeed)
        {
            elapsedTime += Time.unscaledDeltaTime;
            textObject.alpha = Mathf.SmoothStep(currentAlpha, targetAlpha, elapsedTime / fadeSpeed);
            yield return new WaitForEndOfFrame();
        }

        textObject.alpha = targetAlpha;
    }
}
