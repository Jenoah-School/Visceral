using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] protected float health = 100f;
    [SerializeField] private UnityEvent OnHit;
    [SerializeField] private UnityEvent OnDeath;


    protected bool isDead = false;
    protected float startHealth = 100f;

    private void Start()
    {
        startHealth = health;
    }

    public virtual void DealDamage(float damageAmount)
    {
        //Skip is entity is already dead
        if (isDead) return;

        health -= damageAmount;
        if (health <= 0)
        {
            OnDeath.Invoke();
            isDead = true;
        }
        else
        {
            OnHit.Invoke();
        }
    }
}
