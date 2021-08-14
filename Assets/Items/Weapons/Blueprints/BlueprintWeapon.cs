using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintWeapon : Weapon
{
    [SerializeField] BlueprintType BlueprintType;
    [SerializeField] GameObject BlueprintImage; 

    public override void UseWeapon(Vector2 mousePos)
    {
        BlueprintManager.AddBlueprint(VectorRounder.RoundVector(mousePos), BlueprintType);
        Instantiate(BlueprintImage, VectorRounder.RoundVector(mousePos), Quaternion.Euler(Vector2.zero));
    }

    public override WeaponType GetWeaponType()
    {
        return WeaponType.Blueprint;
    }
}
