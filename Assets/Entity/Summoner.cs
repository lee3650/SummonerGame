using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Summoner : MonoBehaviour, IWaveNotifier
{
    [SerializeField] ManaManager ManaManager;
    [SerializeField] CharmManager CharmManager;
    [SerializeField] PlayerIncome PlayerIncome;

    public event Action SummonsChanged = delegate { };
    public event Action FinancialsChanged = delegate { };

    ILivingEntity Entity;
    List<Summon> Summons = new List<Summon>();

    private void Awake()
    {
        Entity = GetComponent<ILivingEntity>();
        WaveSpawner.NotifyWhenWaveEnds(this);
    }

    public void AddCharm(Charm charm)
    {
        CharmManager.AddCharm(charm);
        foreach (Summon s in Summons)
        {
            TryAddCharmToSummon(s, charm);
        }
    }

    public void OnFinancialsChanged()
    {
        FinancialsChanged();
    }

    public List<Summon> GetSummons()
    {
        return Summons; 
    }

    public bool TryReduceMana(float amt)
    {
        return ManaManager.TryDecreaseMana(amt);
    }

    public void AddMana(float amt)
    {
        ManaManager.IncreaseMana(amt);
    }

    void TryAddCharmToSummon(Summon s, Charm charm)
    {
        if (charm.ApplyToType(s.GetSummonType()) && charm.HasAttackModifier())
        {
            s.AddAttackCharm(charm);
        }
    }

    public bool IsPointInSummonRange(Vector2 point)
    {
        foreach (Summon s in Summons)
        {
            if (s.GetSummonType() == SummonType.Miner)
            {
                if (Vector2.Distance(s.transform.position, point) < s.GetComponent<IRanged>().GetRange())
                {
                    return true; 
                }
            }
        }

        return false; 
    }

    public Event GetCharmModifiedEvent(Event e, SummonType type)
    {
        return CharmManager.GetCharmModifiedEvent(e, type);
    }

    public void OnWaveEnds()
    {
        //It's actually important the the player tells the summons that the wave ends and they don't get directly notified because we need to control the order in which you
        //get paid and get, um, your income removed. 
        PlayerIncome.OnWaveEnds();

        for (int i = Summons.Count - 1; i >= 0; i--)
        {
            Summons[i].OnWaveEnds();
        }
    }

    public void OnHit(IEntity hit)
    {
        Entity.OnHit(hit);
    }

    public void OnSummonDeath(float manaCost)
    {
    }
    
    public void AddSummonToParty(Summon s)
    {
        Summons.Add(s);
        print("Charms: " + CharmManager.GetCharms().Count);
        foreach (Charm c in CharmManager.GetCharms())
        {
            TryAddCharmToSummon(s, c);
        }

        SummonsChanged();
    }

    public void RemoveSummonFromParty(Summon s)
    {
        Summons.Remove(s);
        print("Summons changed!");
        SummonsChanged();
    }

    public void MoveSummonsToSummoner()
    {
        foreach (Summon s in Summons)
        {
            s.TryToMoveToSummoner();
        }
    }

    public void RefundManaFromAllLivingSummons()
    {
        foreach (Summon s in Summons)
        {
            if (s.CanRefundMana())
            {
                //ManaManager.IncreaseMaxMana(10f);
                //ManaManager.IncreaseMana(10f);
            }
            if (s != null)
            {
                Destroy(s.gameObject);
            } 
        }

        Summons = new List<Summon>();
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }
}
