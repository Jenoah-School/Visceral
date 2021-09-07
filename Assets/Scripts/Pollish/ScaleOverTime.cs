using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScaleOverTime : MonoBehaviour
{
    [SerializeField] private AnimationCurve scaleCurve = new AnimationCurve(new Keyframe(1f, 1f), new Keyframe(1f, 1f));
    [SerializeField] private float intensity = 1f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private UnityEvent OnBeginCurve;

    private float previousNormalizedTime = 0f;

    private Vector3 startScale = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float normalizedTime = (Time.time * speed) % 1f;

        if(previousNormalizedTime > normalizedTime)
        {
            OnBeginCurve.Invoke();
        }

        transform.localScale = startScale * (scaleCurve.Evaluate(normalizedTime) * intensity);
        previousNormalizedTime = normalizedTime;
    }
}
