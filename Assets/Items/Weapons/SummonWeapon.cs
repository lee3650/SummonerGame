using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonWeapon : Weapon
{
    //so, we just override UseWeapon. 
    //What exactly is the point of the Weapon class? 

    [SerializeField] GameObject Summon;
    public bool ReduceMaxMana;
    
    public override void UseWeapon(Vector2 mousePos)
    {
        GameObject summoned = Instantiate(Summon, mousePos, transform.rotation);
       
        summoned.GetComponent<Summon>().SetSummoner(Wielder.GetComponent<Summoner>());

        ControllableSummon cs;
        if (summoned.TryGetComponent<ControllableSummon>(out cs))
        {
            cs.HoldPoint(mousePos);
        }

        if (ReduceMaxMana)
        {
            summoned.GetComponent<Summon>().ManaRefundAmount = GetManaDrain();
        }
    }

    public override WeaponType GetWeaponType()
    {
        return WeaponType.Summon;
    }
}
