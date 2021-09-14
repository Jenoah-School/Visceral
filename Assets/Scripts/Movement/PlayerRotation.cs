﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private Transform cam = null;
    [SerializeField] private float lookSpeed = 400f;
    [SerializeField] private Vector2 verticalRotationLimit = new Vector2(-90f, 90f);

    private float currentAngleX = 0;
    private float lookMultiplier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if(cam == null)
        {
            cam = Camera.main.transform;
        }
        lookMultiplier = PlayerPrefs.GetFloat("Mouse Sensitivity", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed * lookMultiplier * Time.timeScale;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed * lookMultiplier * Time.timeScale;

        currentAngleX -= mouseY;

        currentAngleX = Mathf.Clamp(currentAngleX, verticalRotationLimit.x, verticalRotationLimit.y);

        cam.localRotation = Quaternion.Euler(currentAngleX, 0, 0);
        transform.localRotation *= Quaternion.Euler(0, mouseX, 0);

    }
}
