using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    [SerializeField] HealthManager HealthManager;
    [SerializeField] SummonType SummonType;

    public event Action SummonerSet = delegate { };

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

    public void Destroy()
    {
        if (HealthManager != null)
        {
            HealthManager.SubtractHealth(10000);
        }
        else
        {
            SummonEnds();
            gameObject.SetActive(false); //let's try this instead of destroying, idk 
        }
    }

    public Summoner GetSummoner()
    {
        return MySummoner;
    }

    public void TryHealSummon(float amt)
    {
        if (HealthManager != null)
        {
            HealthManager.SubtractHealth(-amt);
        }
    }

    public bool GetIsDamaged()
    {
        if (HealthManager != null)
        {
            return HealthManager.GetMaxHealth() == HealthManager.GetCurrent(); 
        }
        return false; 
    }

    public void SetSummoner(Summoner summoner)
    {
        MySummoner = summoner;
        MySummoner.AddSummonToParty(this);
        SummonerSet();
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