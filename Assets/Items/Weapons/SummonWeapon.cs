using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonWeapon : Weapon
{
    //so, we just override UseWeapon. 
    //What exactly is the point of the Weapon class? 

    [SerializeField] GameObject Summon;
    [SerializeField] float rotationOffset; 
    
    public bool ReduceMaxMana;

    public bool ZeroRotation = false; 

    public override void UseWeapon(Vector2 mousePos)
    {
        Quaternion rotation = ZeroRotation ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, rotationOffset));

        GameObject summoned = SpawnSummon(Summon, mousePos, Wielder.GetComponent<Summoner>(), rotation);

        if (ReduceMaxMana)
        {
            summoned.GetComponent<Summon>().ManaRefundAmount = GetManaDrain();
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

        summoned.GetComponent<Summon>().SetSummoner(summoner);

        ControllableSummon cs;
        if (summoned.TryGetComponent<ControllableSummon>(out cs))
        {
            cs.HoldPoint(pos);
        }

        return summoned;
    }
}
