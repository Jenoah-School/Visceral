using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [Header("Distances")]
    [SerializeField] private float followDistance = 8f;
    [SerializeField] private float stopDistance = 3f;

    [Header("Settings")]
    [SerializeField] private Transform target = null;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector3 targetOffset = new Vector3(0, 0.5f, 0f);

    [Header("Rotation")]
    [SerializeField] private bool rotateInMoveDirection = true;
    [SerializeField, Range(0,10)] private float rotationSmoothing = 0.95f;

    private Vector3 velocity = Vector3.zero;
    private Quaternion targetRotation = Quaternion.identity;

    private Vector3 previousPosition = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        if(target == null) target = GameObject.FindGameObjectWithTag("Player").transform;
        if (target == null)
        {
            Debug.LogWarning("No movement target set", gameObject);
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float currentDistanceFromTargetSqr = (transform.position - target.position).sqrMagnitude;
        if(currentDistanceFromTargetSqr < followDistance * followDistance && currentDistanceFromTargetSqr > stopDistance * stopDistance)
        {
            MoveToPosition(target.position + targetOffset);
        }
        if (rotateInMoveDirection)
        {
            velocity = transform.position - previousPosition;
            if(velocity != Vector3.zero) targetRotation = Quaternion.LookRotation(velocity.normalized);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSmoothing * Time.deltaTime);
        }


        previousPosition = transform.position;
    }

    private void MoveToPosition(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, followDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
