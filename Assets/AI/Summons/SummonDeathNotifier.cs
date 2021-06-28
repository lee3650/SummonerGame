using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonDeathNotifier : MonoBehaviour
{
    [SerializeField] HealthManager HealthManager;
    private Summoner MySummoner;
    public float ManaRefundAmount;

    public void SetSummoner(Summoner summoner)
    {
        MySummoner = summoner;
    }

    public void Awake()
    {
        HealthManager.OnDeath += OnDeath;    
    }

    private void OnDeath()
    {
        MySummoner.OnSummonDeath(ManaRefundAmount);
    }
}
