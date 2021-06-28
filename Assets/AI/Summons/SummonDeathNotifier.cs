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
        if (HealthManager != null)
        {
            HealthManager.OnDeath += SummonEnds;    
        }
    }

    protected void SummonEnds()
    {
        MySummoner.OnSummonDeath(ManaRefundAmount);
    }
}
