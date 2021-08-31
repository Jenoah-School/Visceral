using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance = null;

    private Vector3 startPosition = Vector3.zero;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    public void Shake(float duration, float distance, float speed)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeScreen(duration, distance, speed));
    }

    IEnumerator ShakeScreen(float duration, float distance, float speed)
    {
        float elapsedTime = 0f;
        float seed = Time.time;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            Vector3 shakePosition = new Vector3(Mathf.PerlinNoise(seed, elapsedTime * speed) * 2f - 1f, Mathf.PerlinNoise(elapsedTime * speed, seed) * 2f - 1f, 0) * distance;
            shakePosition *= Mathf.SmoothStep(1, 0, elapsedTime / duration);
            transform.localPosition = startPosition + shakePosition;
            yield return new WaitForEndOfFrame();
        }

        transform.localPosition = startPosition;
    }
}
