using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headbob : MonoBehaviour
{
    [SerializeField] private bool isEnabled = true;

    [Header("Bobbing settings")]
    [SerializeField] private float bobMagnitude = 0.015f;
    [SerializeField] private float bobSpeed = 10f;
    [SerializeField] private float minimumMoveSpeed = 3f;
    [SerializeField] private float resetSmoothing = 1f;

    [Header("References")]
    [SerializeField] private Transform referenceCamera = null;
    [SerializeField] private Transform referenceCameraHolder = null;

    private Vector3 startPosition = Vector3.zero;
    private Vector3 previousPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        //if (referenceRigidbody == null) GetComponent<Rigidbody>();
        startPosition = referenceCamera.localPosition;
        previousPosition = referenceCameraHolder.position;
    }

    private void FixedUpdate()
    {
        if (!isEnabled) return;

        CheckMotion();
        ResetPosition();
        //referenceCamera.LookAt(FocusTarget());
        previousPosition = referenceCameraHolder.position;
    }

    private void CheckMotion()
    {
        float playerSpeed = (previousPosition - referenceCameraHolder.position).magnitude;
        if (playerSpeed < minimumMoveSpeed /* || !isGrounded*/) return;

        PlayMotion(FootstepMotion());
    }

    private void PlayMotion(Vector3 motion)
    {
        referenceCamera.localPosition += motion;
    }

    private Vector3 FootstepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * bobSpeed) * bobMagnitude;
        pos.x += Mathf.Cos(Time.time * bobSpeed / 2f) * bobMagnitude * 2;

        return pos;
    }

    private void ResetPosition()
    {
        if (referenceCamera.localPosition == startPosition) return;
        referenceCamera.localPosition = Vector3.Lerp(referenceCamera.localPosition, startPosition, resetSmoothing * Time.deltaTime);
    }
}
