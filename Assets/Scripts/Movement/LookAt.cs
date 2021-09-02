using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private float minimumLookRadius = 10f;
    [SerializeField] private Transform target = null;
    [SerializeField] private float shootCooldown = 3f;
    [SerializeField] private Vector3 targetOffset = Vector3.zero;

    Vector3 playerDirection = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        if (target == null) target = GameObject.FindGameObjectWithTag("Player").transform.root;
        if (target == null)
        {
            Debug.LogWarning("No shooting target set", gameObject);
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerDirection = ((target.position + targetOffset) - transform.position);
        float targetDistance = playerDirection.sqrMagnitude;

        if(targetDistance < minimumLookRadius * minimumLookRadius)
        {
            transform.rotation = Quaternion.LookRotation(playerDirection.normalized);
        }
    }
}
