using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayWithMouse : MonoBehaviour
{
    [Header("Position")]
    [SerializeField] private float positionalStrength = 5f;
    [SerializeField] private float positionalMaxStrength = 10f;
    [SerializeField] private float positionalSmoothing = 1f;

    [Header("Rotation")]
    [SerializeField] private float rotationalStrength = 5f;
    [SerializeField] private float rotationalMaxStrength = 10f;
    [SerializeField] private float rotationalSmoothing = 1f;

    [Header("Constraints")]
    [SerializeField] private bool rotateOnX = true;
    [SerializeField] private bool rotateOnY = true;
    [SerializeField] private bool rotateOnZ = true;

    [Header("Shaking")]
    [SerializeField] private bool idleSway = true;
    [SerializeField] private float shakeStrength = 0.1f;
    [SerializeField] private float shakeSpeed = 0.1f;
    [SerializeField, Range(0, 1000000)] private float noiseSeed = 98546;

    public Vector3 startPosition = Vector3.zero;
    private Vector3 homePosition = Vector3.zero;
    private Vector3 noisePosition = Vector3.zero;

    private Quaternion startRotation = Quaternion.identity;

    private float startShakeStrength = 0.1f;
    private float startShakeSpeed = 0.1f;

    private float mouseX = 0f;
    private float mouseY = 0f;
    private float noiseTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
        homePosition = startPosition;

        startShakeSpeed = shakeSpeed;
        startShakeStrength = shakeStrength;

        noiseSeed += 0.0089f;
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.isPaused) return;
        HandleInput();
        PositionalSway();
        RotationalSway();
    }

    private void HandleInput()
    {
        mouseX = -Input.GetAxis("Mouse X");
        mouseY = -Input.GetAxisRaw("Mouse Y");
    }

    private void PositionalSway()
    {
        float swayX = Mathf.Clamp(mouseX * positionalStrength, -positionalMaxStrength, positionalMaxStrength);
        float swayY = Mathf.Clamp(mouseY * positionalStrength, -positionalMaxStrength, positionalMaxStrength);

        Vector3 targetPosition = new Vector3(swayX, swayY, 0);

        if (idleSway)
        {
            noiseTime += Time.deltaTime * shakeSpeed;

            float xNoise = Mathf.PerlinNoise(noiseTime, noiseSeed) * 2 - 1f;
            float yNoise = Mathf.PerlinNoise(noiseSeed, noiseTime) * 2 - 1f;
            noisePosition = new Vector3(xNoise, yNoise, 0) * shakeStrength;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, noisePosition + targetPosition + homePosition, Time.deltaTime * positionalSmoothing);
    }

    private void RotationalSway()
    {
        float swayX = Mathf.Clamp(mouseY * rotationalStrength, -rotationalMaxStrength, rotationalMaxStrength);
        float swayY = Mathf.Clamp(mouseX * rotationalStrength, -rotationalMaxStrength, rotationalMaxStrength);

        Quaternion targetRotation = Quaternion.Euler(
            rotateOnX ? -swayX : 0f,
            rotateOnY ? swayY : 0f,
            rotateOnZ ? swayY : 0f
            );

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation * startRotation, Time.deltaTime * rotationalSmoothing);
    }

    public void SetHomePosition(Vector3 targetPosition)
    {
        homePosition = targetPosition;
    }

    public void SetNoiseSettings(float strength, float speed)
    {
        shakeSpeed = speed;
        shakeStrength = strength;
    }

    public void ResetHomePosition()
    {
        homePosition = startPosition;
    }

    public void ResetShakeSettings()
    {
        shakeStrength = startShakeStrength;
        shakeSpeed = startShakeSpeed;
    }
}
