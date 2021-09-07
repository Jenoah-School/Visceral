using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Lean.Pool;

public class ShootOnSight : MonoBehaviour
{
    [SerializeField] private float minimumShootDistance = 4f;
    [SerializeField] private float maximumShootDistance = 10f;
    [SerializeField, Range(-1f, 1f)] private float alignmentWeightForShot = 0.8f;
    [SerializeField] private Transform target = null;
    [SerializeField] private float shootCooldown = 2f;
    [SerializeField] private LayerMask ignoreLayers;
    [SerializeField] private Vector3 targetOffset = Vector3.zero;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab = null;
    [SerializeField] private float projectileLifetime = 10f;
    [SerializeField] private Transform spawnPoint = null;

    [Header("Events")]
    [SerializeField] private UnityEvent OnFirstSight;
    [SerializeField] private UnityEvent OnLoseSight;
    [SerializeField] private UnityEvent OnShoot;

    private float nextShootTime = 0f;
    private bool viewIsObstructed = false;
    private Vector3 playerDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null) target = GameObject.FindGameObjectWithTag("Player").transform.root;
        if (target == null)
        {
            Debug.LogWarning("No shooting target set", gameObject);
            enabled = false;
        }

        nextShootTime = Time.time + shootCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInRangeAndView())
        {
            if (viewIsObstructed)
            {
                viewIsObstructed = false;
                OnFirstSight.Invoke();
            }

            if (Time.time < nextShootTime) return;
            if (Vector3.Dot(transform.forward, playerDirection.normalized) > alignmentWeightForShot)
            {
                Shoot();
            }
        }
        else
        {
            if (!viewIsObstructed)
            {
                viewIsObstructed = true;
                OnLoseSight.Invoke();
            }
        }

    }

    public bool IsInRangeAndView()
    {
        playerDirection = ((target.position + targetOffset) - transform.position);
        float targetDistance = playerDirection.sqrMagnitude;

        //When AI can shoot
        if (targetDistance < maximumShootDistance * maximumShootDistance && targetDistance > minimumShootDistance * minimumShootDistance)
        {
            if (Physics.Raycast(transform.position, playerDirection, out RaycastHit hit, maximumShootDistance, ~ignoreLayers))
            {
                //If target is in view
                if (hit.transform.root == target)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void Shoot()
    {
        GameObject projectile = LeanPool.Spawn(projectilePrefab);
        projectile.transform.position = spawnPoint.position;
        projectile.transform.rotation = Quaternion.LookRotation(playerDirection.normalized);
        LeanPool.Despawn(projectile, projectileLifetime);
        nextShootTime = Time.time + shootCooldown;
        OnShoot.Invoke();
    }
}
