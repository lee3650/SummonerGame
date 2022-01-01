using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMinotaur : MonoBehaviour, ISubCharmHandler
{
    [SerializeField] HealthManager HealthManager;
    public void ApplyCharm(Charm c)
    {
        switch (c)
        {
            case MinotaurCharm mc:
                HealthManager.IncreaseMaxHealth(mc.HealthIncrease);
                break; 
        }
    }
}
