using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : EntityHealth
{
    [SerializeField] private Image healthBar = null;
    [SerializeField] private KeyCode reloadKey = KeyCode.R;
    [SerializeField] private float reloadCooldown = 15f;

    private float nextReloadTime = 0;

    private void Start()
    {
        startHealth = health;
        nextReloadTime = Time.time;
    }

    private void Update()
    {
        if (Input.GetKeyDown(reloadKey))
        {
            if (Time.time > nextReloadTime)
            {
                health = startHealth;
                if (healthBar != null) healthBar.fillAmount = 1f / (startHealth / health);
                nextReloadTime = Time.time + reloadCooldown;
            }
        }
    }

    public override void DealDamage(float damageAmount)
    {
        base.DealDamage(damageAmount);
        if (health <= 0)
        {
            if (healthBar != null) healthBar.fillAmount = 0f;
        }
        else
        {
            if (healthBar != null) healthBar.fillAmount = 1f / (startHealth / health);
        }
    }
}
