using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWander : MonoBehaviour
{
    [Header("Distances")]
    [SerializeField] private float minimalWanderDistance = 4f;
    [SerializeField] private float maximumWanderDistance = 10f;

    [Space]
    [SerializeField] private float minimalTargetDistance = 0.4f;

    [Header("Timing")]
    [SerializeField] private float minimumIdleTime = 5f;
    [SerializeField] private float maximumIdleTime = 15f;

    [Header("Settings")]
    [SerializeField] private float terrainHeightCheckDistance = 10f;
    [SerializeField] private LayerMask terrainCheckLayers;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector3 targetOffset = new Vector3(0, 0.5f, 0f);

    private bool isWandering = false;

    private float nextWanderTime = 0f;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 targetRotation = Vector3.zero;
    private Transform player = null;


    // Start is called before the first frame update
    void Start()
    {
        nextWanderTime = Time.time + Random.Range(minimumIdleTime, maximumIdleTime);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWandering && Time.time > nextWanderTime)
        {
            isWandering = true;
            Vector2 tempTargetPos = Random.insideUnitCircle.normalized;
            targetPosition = transform.position + new Vector3(tempTargetPos.x, 0, tempTargetPos.y) * Random.Range(minimalWanderDistance, maximumWanderDistance);

            if (!TrySetTargetLocation(terrainHeightCheckDistance))
            {
                isWandering = false;
                return;
            }

        }
        else if (isWandering)
        {
            //Move to target
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            transform.LookAt(targetPosition, Vector3.up);

            //Check distance to target and if threshold, set to idle and prepare for next wander
            if ((transform.position - targetPosition).sqrMagnitude < minimalTargetDistance * minimalTargetDistance)
            {
                isWandering = false;

                //transform.rotation *= Quaternion.FromToRotation(Vector3.up, targetRotation);
                transform.LookAt(targetPosition, Vector3.up);

                nextWanderTime = Time.time + Random.Range(minimumIdleTime, maximumWanderDistance);
            }
        }
    }

    public void WanderInDirection(Vector3 targetDirection)
    {
        targetDirection.y = 0;
        targetDirection.Normalize();

        targetPosition = transform.position + targetDirection * maximumWanderDistance;
        if (!TrySetTargetLocation(30f))
        {
            nextWanderTime = Time.time - 1f;
            isWandering = false;
        }
        else
        {
            isWandering = true;
        }
    }

    private bool TrySetTargetLocation(float terrainHeightCheckDistance)
    {
        if (Physics.Raycast(targetPosition + (Vector3.up * terrainHeightCheckDistance), Vector3.down, out RaycastHit hit, terrainHeightCheckDistance * 1.5f, terrainCheckLayers))
        {
            targetPosition.y = hit.point.y;
            targetPosition += targetOffset;
            targetRotation = hit.normal;
            return true;
        }
        return false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (player == null || !other.transform.CompareTag("Player")) return;
        if ((transform.position - player.transform.position).sqrMagnitude < minimalTargetDistance * minimalTargetDistance)
        {
            targetPosition = transform.position;
            return;
        }

        targetPosition = player.transform.position;
        if (!TrySetTargetLocation(terrainHeightCheckDistance))
        {
            nextWanderTime = Time.time - 1f;
            isWandering = false;
        }
        else
        {
            isWandering = true;
        }
    }
}
