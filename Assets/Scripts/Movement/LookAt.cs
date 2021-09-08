using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private float minimumLookRadius = 10f;
    [SerializeField] private Transform target = null;
    [SerializeField] private Vector3 targetOffset = Vector3.zero;
    [SerializeField] private float rotationSmoothing = 5f;
    [SerializeField] private float lookClamping = 45f;

    Vector3 playerDirection = Vector3.zero;
    Quaternion targetRotation = Quaternion.identity;
    Vector3 startForward = Vector3.zero;
    private float normalizedLookClamping = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        if (target == null) target = GameObject.FindGameObjectWithTag("Player").transform.root;
        if (target == null)
        {
            Debug.LogWarning("No shooting target set", gameObject);
            enabled = false;
        }
        startForward = transform.forward;
        normalizedLookClamping = 1f - lookClamping / 180f;
    }

    // Update is called once per frame
    void Update()
    {
        playerDirection = ((target.position + targetOffset) - transform.position);
        float targetDistance = playerDirection.sqrMagnitude;

        if(Vector3.Dot(startForward, playerDirection.normalized) > normalizedLookClamping && targetDistance < minimumLookRadius * minimumLookRadius)
        {
            targetRotation = Quaternion.LookRotation(playerDirection.normalized);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSmoothing * Time.deltaTime);
    }
}
