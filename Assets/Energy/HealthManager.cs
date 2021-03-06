using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour, IEnergyManager
{
    [SerializeField] float MaxHealth;
    [SerializeField] float CurrentHealth;

    public event Action OnDeath = delegate { };
    public event Action OnHealthChanged = delegate { };
    public event Action OnDamageTaken = delegate { };

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public bool IsAlive()
    {
        return CurrentHealth > 0f;
    }

    public void HealToFull()
    {
        CurrentHealth = MaxHealth;
        OnHealthChanged();
    }

    public void IncreaseMaxHealth(float amount)
    {
        MaxHealth += amount;
        HealToFull();
    }

    public void Heal(float amt)
    {
        CurrentHealth += amt; 
        //we could consider not doing this because it does somewhat reward you for keeping entities alive... 
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth; 
        }
        OnHealthChanged();
    }

    public bool IsHealthGreaterThan(float amt)
    {
        return CurrentHealth > amt; 
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
        OnHealthChanged();
        OnDamageTaken();
        if (CurrentHealth <= 0f)
        {
            OnDeath();
        }
    }

    public float GetMax()
    {
        return GetMaxHealth();
    }
    public float GetRemainingPercentage()
    {
        return GetHealthPercentage();
    }
    public float GetCurrent()
    {
        return CurrentHealth;
    }
 }
