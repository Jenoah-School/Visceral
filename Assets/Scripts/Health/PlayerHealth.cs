using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class PlayerHealth : EntityHealth
{
    [SerializeField] private Image healthBar = null;
    [SerializeField] private Image reloadBar = null;
    [SerializeField] private KeyCode reloadKey = KeyCode.R;
    [SerializeField] private float reloadCooldown = 15f;
    [SerializeField] private float projectileDamage = 10f;

    [SerializeField] private UnityEvent OnHeal;

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
                if (healthBar != null) healthBar.DOFillAmount(1f, 1.5f);
                reloadBar.fillAmount = 0f;
                nextReloadTime = Time.time + reloadCooldown;
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
