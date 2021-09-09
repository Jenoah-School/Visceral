using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class PlayerHealth : EntityHealth
{
    [Header("UI")]
    [SerializeField] private Image healthBar = null;
    [SerializeField] private Image reloadBar = null;

    [Header("Reloading")]
    [SerializeField] private KeyCode reloadKey = KeyCode.R;
    [SerializeField] private float reloadCooldown = 15f;

    [Header("Health settings")]
    [SerializeField] private float projectileDamage = 10f;
    [SerializeField] private float lowHealthPercentage = 30f;

    [SerializeField] private UnityEvent OnHeal;
    [SerializeField] private UnityEvent OnLowHealth;

    private float nextReloadTime = 0;
    private float lowHealthValue = 30;
    private bool isLowHealth = false;

    private void Start()
    {
        startHealth = health;
        lowHealthValue = health / 100f * lowHealthValue;
        nextReloadTime = Time.time;
    }

    private void Update()
    {
        if (Input.GetKeyDown(reloadKey))
        {
            if (Time.time > nextReloadTime)
            {
                health = startHealth;
                if (healthBar != null) healthBar.DOFillAmount(1f, 1.5f);
                reloadBar.fillAmount = 0f;
                nextReloadTime = Time.time + reloadCooldown;
                isLowHealth = false;
                OnHeal.Invoke();
            }
        }

        if (reloadBar != null && Time.time < nextReloadTime)
        {
            float timeRemaining = Time.time - nextReloadTime;
            float normalizedRemaining = 1f - Mathf.Abs(timeRemaining / reloadCooldown);
            reloadBar.fillAmount = normalizedRemaining;
        }
    }

    public override void DealDamage(float damageAmount)
    {
        base.DealDamage(damageAmount);
        healthBar.DOKill();
        if (health <= 0)
        {
            if (healthBar != null) healthBar.DOFillAmount(0f, 0.1f);
        }
        else
        {
            if (healthBar != null) healthBar.DOFillAmount(1f / (startHealth / health), 0.1f);
            if(!isLowHealth && health < lowHealthValue)
            {
                isLowHealth = true;
                OnLowHealth.Invoke();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
            DealDamage(projectileDamage);
        }
    }
}
