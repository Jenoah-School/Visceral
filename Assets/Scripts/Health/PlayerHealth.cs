using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : EntityHealth
{
    [SerializeField] private Image healthBar = null;

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
