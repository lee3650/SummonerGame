using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaManager : MonoBehaviour, IEnergyManager
{
    [SerializeField] float MaxMana;
    [SerializeField] float CurrentMana;
    [SerializeField] float ManaIncrement;
    [SerializeField] float ManaRegenTickRate;

    float timer = 0f;

    public void Start()
    {
        CurrentMana = MaxMana;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= ManaRegenTickRate)
        {
            timer = 0f;
            IncreaseMana(ManaIncrement);
        }
    }

    public void IncreaseMana(float amt)
    {
        CurrentMana += amt;
        if (CurrentMana > MaxMana)
        {
            CurrentMana = MaxMana;
        }
    }
    
    public void DecreaseMana(float amt)
    {
        CurrentMana -= amt;
        if (CurrentMana < 0)
        {
            throw new System.Exception("Mana was overdrained!");
        }
    }

    public void DecreaseMaxMana(float amt)
    {
        MaxMana -= amt; 
        if (CurrentMana > MaxMana)
        {
            CurrentMana = MaxMana;
        }
    }

    public void IncreaseMaxMana(float amt)
    {
        MaxMana += amt;
    }

    public bool IsManaMoreThanOrEqual(float amt)
    {
        if (CurrentMana >= amt)
        {
            return true;
        }
        return false; 
    }

    public bool TryDecreaseMana(float amt)
    {
        if (CurrentMana >= amt)
        {
            DecreaseMana(amt);
            return true; 
        }
        return false;
    }

    public float GetMax()
    {
        return MaxMana;
    }
    public float GetRemainingPercentage()
    {
        return CurrentMana/MaxMana;
    }
    public float GetCurrent()
    {
        return CurrentMana;
    }
}
