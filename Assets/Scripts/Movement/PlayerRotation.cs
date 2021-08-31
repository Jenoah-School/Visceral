using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private Transform cam = null;
    [SerializeField] private float verticalRotationSpeed = 400f;
    [SerializeField] private float horizontalRotationSpeed = 400f;
    [SerializeField] private Vector2 verticalRotationLimit = new Vector2(-90f, 90f);

    private float currentVerticalAngle = 0;
    private float currentHorizontalAngle = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(cam == null)
        {
            cam = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentVerticalAngle -= Input.GetAxis("Mouse Y") * Time.deltaTime * verticalRotationSpeed;
        currentHorizontalAngle += Input.GetAxis("Mouse X") * horizontalRotationSpeed * Time.deltaTime;

        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, verticalRotationLimit.x, verticalRotationLimit.y);

        cam.localRotation = Quaternion.Euler(currentVerticalAngle, 0, 0);
        transform.localRotation = Quaternion.Euler(0, currentHorizontalAngle, 0);

    }
}
