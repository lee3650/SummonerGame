using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    [SerializeField] HealthManager HealthManager;
    [SerializeField] SummonType SummonType;

    IDamager IDamager; 

    private Summoner MySummoner;
    public float ManaRefundAmount;

    public void Awake()
    {
        if (HealthManager != null)
        {
            HealthManager.OnDeath += SummonEnds;
        }

        IDamager = GetComponent<IDamager>();
        if (IDamager == null)
        {
            IDamager = GetComponentInChildren<IDamager>();
        }
    }

    public void SetSummoner(Summoner summoner)
    {
        MySummoner = summoner;
        MySummoner.AddSummonToParty(this);
    }

    public Vector2 GetSummonerPosition()
    {
        return MySummoner.GetPosition();
    }

    public Event GetCharmModifiedEvent(Event e)
    {
        return MySummoner.GetCharmModifiedEvent(e, GetSummonType());
    }

    public void AddAttackCharm(Charm charm)
    {
        if (IDamager != null)
        {
            IDamager.AddAttackModifier(charm.GetAttackModifier());
        }
    }

    public SummonType GetSummonType()
    {
        return SummonType;
    }

    public virtual void TryToMoveToSummoner()
    {
        transform.position = 2 * UnityEngine.Random.insideUnitCircle + MySummoner.GetPosition(); 
    }

    public virtual bool CanRefundMana()
    {
        return false; 
    }

    protected virtual void SummonEnds()
    {
        MySummoner.OnSummonDeath(ManaRefundAmount);
        MySummoner.RemoveSummonFromParty(this);
    }
}