using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour
{
    [SerializeField] HealthManager HealthManager;

    private void Start()
    {
        if (GetComponent<ITargetable>() != null)
        {   
            FlammableManager.AddFlammable(GetComponent<ITargetable>());
        }

        if (HealthManager != null)
        {
            HealthManager.OnDeath += OnDeath;
        }
    }

    private void OnDeath()
    {
        FlammableManager.RemoveFlammable(GetComponent<ITargetable>());
    }
}
