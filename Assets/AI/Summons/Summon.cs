using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    [SerializeField] HealthManager HealthManager;

    private Summoner MySummoner;
    public float ManaRefundAmount;

    public void SetSummoner(Summoner summoner)
    {
        MySummoner = summoner;
        MySummoner.AddSummonToParty(this);
    }

    public Vector2 GetSummonerPosition()
    {
        return MySummoner.GetPosition();
    }

    public virtual void TryToMoveToSummoner()
    {
        transform.position = 2 * UnityEngine.Random.insideUnitCircle + MySummoner.GetPosition(); 
    }

    public void Awake()
    {
        if (HealthManager != null)
        {
            HealthManager.OnDeath += SummonEnds;
        }
    }

    protected virtual void SummonEnds()
    {
        MySummoner.OnSummonDeath(ManaRefundAmount);
        MySummoner.RemoveSummonFromParty(this);
    }
}