using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeOverDistance : MonoBehaviour
{
    [SerializeField] private float maxDistance = 15f;
    [SerializeField] private float explosionForce = 5f;
    [SerializeField] private float explosionDuration = 0.75f;
    [SerializeField] private int explosionVibration = 4;

    // Start is called before the first frame update
    void Start()
    {
        if (ScreenShake.Instance == null) return;
        Vector3 playerPosition = Camera.main.transform.position;
        Vector3 playerDirection = transform.position - playerPosition;
        float normalizedDistance = playerDirection.sqrMagnitude / (maxDistance * maxDistance);
        ScreenShake.Instance.Shake(explosionDuration, explosionForce, explosionVibration);
    }
}
