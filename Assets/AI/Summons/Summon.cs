using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    [SerializeField] protected HealthManager HealthManager;
    [SerializeField] SummonType SummonType;
    [SerializeField] bool HealToFull = false; 
    [SerializeField] protected float WaveHealAmt = 10f;
    [SerializeField] bool upgradable = true;
    [SerializeField] int summonTier = 0; 

    public event Action SummonerSet = delegate { };
    public event Action SummonWaveEnds = delegate { };

    IDamager IDamager; 

    private Summoner MySummoner;
    public float ManaRefundAmount;

    public void Awake()
    {
        //Watch out if you're changing this - the Home Tile is kind of depending on it not doing anything important 

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
    
    public float GetIncome()
    {
        IEarner earner;
        if (TryGetComponent<IEarner>(out earner))
        {
            return earner.GetIncome();
        }
        return 0f; 
    }

    public float GetMaintenanceFee()
    {
        IRecurringCost rc;
        if (TryGetComponent<IRecurringCost>(out rc))
        {
            return rc.GetRecurringCost(); 
            //so, this is fine, I guess, but the problem is it isn't enforced
            //something can draw a recurring fee without implementing that interface. It'd be better if the only way you could 
            //have a maintenance fee would be to implement that interface, right. 
        }
        return 0f; 
    }

    public void InitializeScripts()
    {
        IInitialize[] inits = GetComponents<IInitialize>();
        foreach (IInitialize i in inits)
        {
            i.Init();
        }
    }

    public virtual void OnWaveEnds()
    {
        TryHealSummon(WaveHealAmt);
        
        SummonWaveEnds();
    }

    public void Destroy()
    {
        if (HealthManager != null)
        {
            HealthManager.SubtractHealth(10000);
            gameObject.SetActive(false);
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
            if (HealToFull)
            {
                HealthManager.HealToFull();
            } else
            {
                HealthManager.Heal(amt);
            }
        }
    }

    public bool GetIsDamaged()
    {
        if (HealthManager != null)
        {
            return HealthManager.GetCurrent() < HealthManager.GetMaxHealth(); 
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

    public void SetHealthManager(HealthManager hm)
    {
        HealthManager = hm;
        Awake();
    }

    public int SummonTier
    {
        get
        {
            return summonTier;
        }
    }

    public bool Upgradable
    {
        get
        {
            return upgradable;
        }
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