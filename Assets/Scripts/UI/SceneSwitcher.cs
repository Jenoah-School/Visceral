using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private float fadeDuration = 1.25f;

    private bool isTransitioning = false;

    public void SwitchScene(int buildIndex)
    {
        if (!isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(SwitchWithFade(buildIndex));
        }
    }

    private IEnumerator SwitchWithFade(int buildIndex)
    {
        fadeAnimator.SetTrigger("FadeIn");
        yield return new WaitForSecondsRealtime(fadeDuration);
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Single);
    }
}
