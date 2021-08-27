using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonWeapon : Weapon
{
    //so, we just override UseWeapon. 
    //What exactly is the point of the Weapon class? 

    public const float RefundPercent = 0.67f;

    [SerializeField] protected GameObject Summon;
    [SerializeField] float rotationOffset; 
    
    public bool ReduceMaxMana;

    public bool ZeroRotation = false;

    private GameObject SummonPreview; 

    public void UpdatePreview(bool visible, Vector2 mousePos)
    {
        if (visible)
        {
            SummonPreview.SetActive(true);
            SummonPreview.transform.position = VectorRounder.RoundVector(mousePos);
        } else
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

    public override WeaponType GetWeaponType()
    {
        return WeaponType.Summon;
    }

    public static GameObject SpawnSummon(GameObject summon, Vector2 pos, Summoner summoner, Quaternion rotation)
    {
        GameObject summoned = Instantiate(summon, pos, rotation);

        Summon summonComponent = summoned.GetComponent<Summon>();

        summonComponent.InitializeScripts();
        summonComponent.SetSummoner(summoner);

        ControllableSummon cs;
        if (summoned.TryGetComponent<ControllableSummon>(out cs))
        {
            cs.HoldPoint(pos);
        }

        return summoned;
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
