using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaManager : MonoBehaviour
{
    [SerializeField] float CurrentMana;
    [SerializeField] float ManaIncrement;
    [SerializeField] float ManaRegenTickRate;

    float timer = 0f;

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
    }
    
    public void DecreaseMana(float amt)
    {
        CurrentMana -= amt;
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
    
    public float GetCurrent()
    {
        return (float)CurrentMana;
    }
}
