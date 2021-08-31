using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Transform objectToFollow = null;

    [Header("Position")]
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private bool isInWorldSpace = true;

    // Update is called once per frame
    void Update()
    {
        if (objectToFollow == null) return;
        transform.position = isInWorldSpace || offset == Vector3.zero ? objectToFollow.position + offset : objectToFollow.TransformPoint(offset);
    }
}
