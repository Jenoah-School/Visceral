using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInRadius : MonoBehaviour
{
    [SerializeField] private Transform player = null;
    [SerializeField] private float damageRadius = 3f;
    [SerializeField] private float damageTime = 3f;
    [SerializeField] private float damageAmount = 5f;

    private float nextDamageTime = 0f;
    private PlayerHealth playerHealth = null;

    private void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        if (player == null || playerHealth == null) enabled = false;
        nextDamageTime = Time.time + damageTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < nextDamageTime) return;
        if ((transform.position - player.transform.position).sqrMagnitude < damageRadius * damageRadius)
        {
            playerHealth.DealDamage(damageAmount);
            nextDamageTime = Time.time + damageTime;
        }
    }
}
