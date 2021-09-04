using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintWeapon : SummonWeapon
{
    [SerializeField] BlueprintType BlueprintType;
    List<GameObject> blueprintImages = new List<GameObject>();

    private void Awake()
    {
        blueprintImages = new List<GameObject>();
        BlueprintManager.BlueprintsChanged += BlueprintsChanged;
    }

    private void BlueprintsChanged()
    {
        for (int i = blueprintImages.Count - 1; i >= 0; i--)
        {
            if (BlueprintManager.ShouldRemoveSummon(blueprintImages[i].transform.position, BlueprintType))
            {
                GameObject g = blueprintImages[i];
                blueprintImages[i] = null;
                blueprintImages.RemoveAt(i);
                Destroy(g);
            }
        }
    }

    public override void UseWeapon(Vector2 mousePos)
    {
        BlueprintManager.AddBlueprint(VectorRounder.RoundVector(mousePos), BlueprintType);
        GameObject b = Instantiate(Summon, VectorRounder.RoundVector(mousePos), Quaternion.Euler(Vector2.zero));
        blueprintImages.Add(b);
        RangeVisualizer rv; 
        if (b.TryGetComponent<RangeVisualizer>(out rv))
        {
            rv.Hide();
        }
    }

    public override WeaponType GetWeaponType()
    {
        return WeaponType.Blueprint;
    }
}
