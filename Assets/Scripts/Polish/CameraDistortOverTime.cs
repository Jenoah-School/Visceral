using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraDistortOverTime : MonoBehaviour
{
    [SerializeField] private Volume postProcessingVolume;
    [SerializeField] private AnimationCurve pulseCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(.5f, 0f), new Keyframe(1f, 1f));
    [SerializeField] private float speed = 3f;
    [SerializeField] private float intensity = 1f;
    [SerializeField] private float distanceBeforeActivation = 20f;
    [SerializeField] private AnimationCurve distanceIntensityCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

    [SerializeField] private Transform intensityTarget = null;
    private float dotIntensity = 1f;
    private float targetDistance = 1f;

    private LensDistortion lensDistortion;

    // Start is called before the first frame update
    void Start()
    {
        postProcessingVolume.profile.TryGet(out lensDistortion);
    }

    // Update is called once per frame
    void Update()
    {
        float normalizedTime = (Time.time * speed) % 1f;

        if (intensityTarget != null)
        {
            Vector3 targetDirection = (intensityTarget.position - transform.position);
            dotIntensity = Mathf.Clamp01(Vector3.Dot(transform.forward, targetDirection.normalized));
            targetDistance = targetDirection.sqrMagnitude > distanceBeforeActivation * distanceBeforeActivation ? 0f : 1f - distanceIntensityCurve.Evaluate(targetDirection.sqrMagnitude / (distanceBeforeActivation * distanceBeforeActivation));
        }
        lensDistortion.intensity.value = pulseCurve.Evaluate(normalizedTime) * (intensity * dotIntensity * targetDistance);
    }
}
