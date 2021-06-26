using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] float MaxHealth;
    [SerializeField] float CurrentHealth;

    public event Action OnDeath = delegate { };
    public event Action OnDamageTaken = delegate { };

    private void Start()
    {
        CurrentHealth = MaxHealth;        
    }

    public bool IsAlive()
    {
        return CurrentHealth > 0f;
    }

    public float GetHealthPercentage()
    {
        return CurrentHealth / MaxHealth;
    }

    public float GetMaxHealth()
    {
        return MaxHealth;
    }

    public void SubtractHealth(float damage)
    {
        CurrentHealth -= damage;
        OnDamageTaken();
        if (CurrentHealth <= 0f)
        {
            OnDeath();
        }
    }
}
