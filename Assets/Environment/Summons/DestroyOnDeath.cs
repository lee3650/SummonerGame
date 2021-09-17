using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    [SerializeField] Summon MySummon;
    [SerializeField] HealthManager HealthManager;

    private void Awake()
    {
        HealthManager.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        HealthManager.OnDeath -= OnDeath;
        MySummon.Destroy();
    }
}
