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

        GameObject summoned = Instantiate(Summon, mousePos, rotation);
       
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
