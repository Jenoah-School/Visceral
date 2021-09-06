using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(NavMeshAgent))]
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
    [SerializeField, Range(0, 10)] private float rotationSmoothing = 0.95f;
    [SerializeField] private float speedBeforeLookAt = 1f;

    private Vector3 velocity = Vector3.zero;
    private Quaternion targetRotation = Quaternion.identity;
    private NavMeshAgent navMeshAgent = null;

    private bool canMove = true;


    // Start is called before the first frame update
    void Start()
    {
        if (target == null) target = GameObject.FindGameObjectWithTag("Player").transform;
        if (target == null)
        {
            Debug.LogWarning("No movement target set", gameObject);
            enabled = false;
        }

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = stopDistance;
        navMeshAgent.speed = moveSpeed;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 5f))
        {
            if (hit.distance > 0.4f)
            {
                transform.position = hit.point + Vector3.up * navMeshAgent.height / 2f;
            }
        }
    }

    private void OnDisable()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;
    }

    private void OnEnable()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float currentDistanceFromTargetSqr = (transform.position - target.position).sqrMagnitude;
        if (canMove)
        {
            if (currentDistanceFromTargetSqr < followDistance * followDistance && currentDistanceFromTargetSqr > stopDistance * stopDistance)
            {
                //MoveToPosition(target.position + targetOffset);
                navMeshAgent.SetDestination(target.position + targetOffset);
            }
        }

        if (rotateInMoveDirection)
        {
            if (currentDistanceFromTargetSqr < followDistance * followDistance || navMeshAgent.velocity == Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(((target.position + targetOffset) - transform.position));
            }
            else
            {
                targetRotation = Quaternion.LookRotation(navMeshAgent.velocity.normalized);
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSmoothing * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, followDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }

    public void SetMoveState(bool moveState)
    {
        canMove = moveState;
    }
}
