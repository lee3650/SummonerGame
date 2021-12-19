using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonWeapon : Weapon, IFinancialPreviewer
{
    //so, we just override UseWeapon. 
    //What exactly is the point of the Weapon class? 

    public const float RefundPercent = 0.67f;

    [SerializeField] protected GameObject Summon;
    [SerializeField] float rotationOffset;
    [SerializeField] bool UseSummonImage = true;
    [SerializeField] bool SetColorToWhite = false;

    public bool ReduceMaxMana;

    public bool ZeroRotation = false;

    protected GameObject SummonPreview;

    protected virtual void Awake()
    {
        if (UseSummonImage)
        {
            SpriteRenderer sr;
            if (Summon.TryGetComponent<SpriteRenderer>(out sr))
            {
                GetComponent<SpriteRenderer>().sprite = sr.sprite;
                if (SetColorToWhite)
                {
                    GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                {
                    GetComponent<SpriteRenderer>().color = sr.color;
                }
            }
        }
    }

    public virtual float EffectOnBalance()
    {
        return GetManaDrain();
    }

    public virtual float EffectOnIncome(Vector2 pos)
    {
        PlayerMiner miner = Summon.GetComponent<PlayerMiner>();

        if (miner != null)
        {
            return miner.GetIncomePreview(pos);
        }

        return GetRecurringCost();
    }

    public Sprite GetSummonSprite()
    {
        SpriteRenderer s;
        if (Summon.TryGetComponent<SpriteRenderer>(out s))
        {
            return s.sprite;
        }
        return null;
    }

    public override string GetDescription()
    {
        IControllableSummon cs;
        if (Summon.TryGetComponent<IControllableSummon>(out cs))
        {
            return FormatStringWidth(WeaponDescription + "\n\n" + cs.GetStatString(Vector2.zero));
        }
        else
        {
            return base.GetDescription();
        }
    }

    public virtual void UpdatePreview(bool visible, Vector2 mousePos)
    {
        if (visible)
        {
            SummonPreview.SetActive(true);
            SummonPreview.transform.position = VectorRounder.RoundVector(mousePos);
        }
        else
        {
            SummonPreview.SetActive(false);
        }
    }

    public override void OnSelection()
    {
        if (SummonPreview == null)
        {
            SummonPreview = Instantiate(Summon);
            SummonPreview.layer = LayerMask.NameToLayer("SummonPreview");
            Collider[] cols = SummonPreview.GetComponents<Collider>();
            foreach (Collider c in cols)
            {
                c.enabled = false;
            }

            Behaviour[] bs = SummonPreview.GetComponents<Behaviour>();
            foreach (Behaviour b in bs)
            {
                b.enabled = false;
            }
        }
        base.OnSelection();
    }

    public override void OnDeselection()
    {
        SummonPreview.SetActive(false);
        base.OnDeselection();
    }

    public override void UseWeapon(Vector2 mousePos)
    {
        Quaternion rotation = ZeroRotation ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, rotationOffset));

        GameObject summoned = SpawnSummon(Summon, VectorRounder.RoundVector(mousePos), Wielder.GetComponent<Summoner>(), rotation);

        if (ReduceMaxMana)
        {
            summoned.GetComponent<Summon>().ManaRefundAmount = GetManaDrain();
        }

        Sellable sellable;
        if (summoned.TryGetComponent<Sellable>(out sellable))
        {
            sellable.SellPrice = Mathf.RoundToInt(GetManaDrain() * RefundPercent);
        }

        if (FirstUseFree)
        {
            FirstUseFree = false; 
        }
    }

    public override float GetRecurringCost()
    {
        IRecurringCost purchasable;

        if (Summon.TryGetComponent<IRecurringCost>(out purchasable))
        {
            return purchasable.GetRecurringCost();
        }

        return base.GetRecurringCost();
    }

    public static GameObject SpawnSummon(GameObject summon, Vector2 pos, Summoner summoner, Quaternion rotation)
    {
        GameObject summoned = Instantiate(summon, pos, rotation);
        Summon summonComponent = summoned.GetComponent<Summon>();

        InitializeSpawnedSummon(summonComponent, summoner);

        return summoned;
    }

    public static void InitializeSpawnedSummon(Summon summon, Summoner summoner)
    {
        summon.InitializeScripts();
        summon.SetSummoner(summoner);

        ControllableSummon cs;
        if (summon.TryGetComponent<ControllableSummon>(out cs))
        {
            cs.HoldPoint(summon.transform.position);
        }
    }

    public static void UpgradeSummon(UpgradePath uc, Vector2 position, Summoner summoner, Sellable sellable)
    {
        GameObject next = SummonWeapon.SpawnSummon(uc.GetNextSummon(), position, summoner, Quaternion.Euler(Vector2.zero));

        Sellable s;
        if (next.TryGetComponent<Sellable>(out s))
        {
            s.SellPrice = sellable.SellPrice + Mathf.RoundToInt(uc.GetUpgradeCost() * RefundPercent);
        }
    }
}
