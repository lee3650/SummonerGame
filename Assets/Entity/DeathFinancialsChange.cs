using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFinancialsChange : MonoBehaviour
{
    [SerializeField] HealthManager HealthManager;
    [SerializeField] Summon Summon;
    private void Awake()
    {
        HealthManager.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        Summon.GetSummoner().OnFinancialsChanged();
    }
}
