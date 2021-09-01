using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private UnityEvent OnHit;
    [SerializeField] private UnityEvent OnDeath;
    [SerializeField] private Image healthBar = null;

    private bool isDead = false;
    private float startHealth = 100f;

    private void Start()
    {
        startHealth = health;
    }

    public void DealDamage(float damageAmount)
    {
        //Skip is player is already dead
        if (isDead) return;

        health -= damageAmount;
        if (health <= 0)
        {
            OnDeath.Invoke();
            if (healthBar != null) healthBar.fillAmount = 0f;
            isDead = true;
        }
        else
        {
            OnHit.Invoke();
            if (healthBar != null) healthBar.fillAmount = startHealth / health;
        }
    }
}
