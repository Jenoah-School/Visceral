using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOnHandheld : MonoBehaviour
{
    [SerializeField, Range(0, 179)] private float zoomFOVTarget = 40f;
    [SerializeField] private float zoomTime = 0.3f;

    private float startFOV = 60f;

    private Camera mainCamera = null;
    private bool isZoomed = false;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        startFOV = mainCamera.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.isPaused) return;
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ZoomIn();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            ZoomOut();
        }

        if(isZoomed && !Input.GetMouseButton(1)) ZoomOut();
    }

    private void ZoomIn()
    {
        StopAllCoroutines();
        StartCoroutine(SetFOV(zoomFOVTarget));
        isZoomed = true;
    }

    private void ZoomOut()
    {
        StopAllCoroutines();
        StartCoroutine(SetFOV(startFOV));
        isZoomed = false;
    }

    IEnumerator SetFOV(float targetFOV)
    {
        float currentFOV = mainCamera.fieldOfView;
        float elapsedTime = 0f;

        while(elapsedTime < zoomTime)
        {
            elapsedTime += Time.deltaTime;
            mainCamera.fieldOfView = Mathf.SmoothStep(currentFOV, targetFOV, elapsedTime / zoomTime);
            yield return new WaitForEndOfFrame();
        }

        mainCamera.fieldOfView = targetFOV;
    }
}
