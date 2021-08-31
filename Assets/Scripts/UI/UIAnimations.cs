using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class UIAnimations : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private float animationTime = .75f;
    [SerializeField, Range(0f, 4f)] private float widthMultiplier = 1.5f;
    [SerializeField, Range(0f, 4f)] private float heightMultiplier = 1f;
    [SerializeField] private bool moveCenter = true;

    [Header("Movement")]
    [SerializeField] private Vector2 targetPosition = Vector2.zero;

    [Header("Audio")]
    [SerializeField] private AudioClip hoverSound = null;
    [SerializeField] private AudioClip clickDownSound = null;
    [SerializeField] private AudioClip clickUpSound = null;
    [SerializeField] private AudioClip fullClickSound = null;

    private Vector2 startSize = new Vector2();
    private Vector2 startPosition = new Vector2();
    private RectTransform rectTransform = null;
    private AudioSource audioSource = null;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startSize = rectTransform.sizeDelta;
        startPosition = rectTransform.anchoredPosition;
        audioSource = GetComponent<AudioSource>();
    }

    public void GrowToSize()
    {
        Vector2 targetSize = startSize;
        targetSize.x *= widthMultiplier;
        targetSize.y *= heightMultiplier;

        Vector2 targetPosition = startPosition;
        targetPosition.x += (targetSize.x - startSize.x) / 2f;
        targetPosition.y += (targetSize.y - startSize.y) / 2f;

        StopAllCoroutines();
        StartCoroutine(ChangeSize(targetSize, targetPosition));
    }

    public void ReturnToStartSize()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeSize(startSize, startPosition));
    }

    public void PlayClickHoverSound()
    {
        if (hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void PlayClickDownSound()
    {
        if (clickDownSound != null)
        {
            audioSource.PlayOneShot(clickDownSound);
        }
    }

    public void PlayClickUpSound()
    {
        if (clickUpSound != null)
        {
            audioSource.PlayOneShot(clickUpSound);
        }
    }

    public void PlayClickFullClickound()
    {
        if (fullClickSound != null)
        {
            audioSource.PlayOneShot(fullClickSound);
        }
    }

    public void MoveToPosition()
    {
        StopAllCoroutines();
        StartCoroutine(MoveToPosition_IEnum(targetPosition));
    }

    public void MoveToStartPosition()
    {
        StopAllCoroutines();
        StartCoroutine(MoveToPosition_IEnum(startPosition));
    }

    public void MoveToStartPositionImmediately()
    {
        if (rectTransform != null)
            rectTransform.anchoredPosition = startPosition;
    }

    public void MoveToTargetPositionImmediately()
    {
        if (rectTransform != null)
            rectTransform.anchoredPosition = targetPosition;
    }

    private IEnumerator ChangeSize(Vector2 targetSize, Vector2 targetPosition)
    {
        float elapsedTime = 0f;
        Vector2 currentSize = rectTransform.sizeDelta;
        Vector2 currentPosition = rectTransform.anchoredPosition;

        float normalizedTime = 0f;

        while (elapsedTime < animationTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            normalizedTime = Mathf.SmoothStep(0f, 1f, elapsedTime / animationTime);
            rectTransform.sizeDelta = Vector2.Lerp(currentSize, targetSize, normalizedTime);
            if (moveCenter)
            {
                rectTransform.anchoredPosition = Vector2.Lerp(currentPosition, targetPosition, normalizedTime);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator MoveToPosition_IEnum(Vector2 targetPosition)
    {
        float elapsedTime = 0f;
        Vector2 currentPosition = rectTransform.anchoredPosition;

        while (elapsedTime < animationTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            rectTransform.anchoredPosition = Vector2.Lerp(currentPosition, targetPosition, Mathf.SmoothStep(0f, 1f, elapsedTime / animationTime));
            yield return new WaitForEndOfFrame();
        }
    }

}
