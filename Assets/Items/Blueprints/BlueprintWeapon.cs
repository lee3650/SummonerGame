using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintWeapon : SummonWeapon
{
    [SerializeField] BlueprintType BlueprintType;
    [SerializeField] bool HasRequiredAdjacents = true;
    [SerializeField] List<TileType> RequiredAdjacentTiles;
    [SerializeField] List<BlueprintType> RequiredAdjacents;
    [SerializeField] List<BlueprintType> BlacklistedAdjacents;
    [SerializeField] float MaintenanceFee;

    [SerializeField] bool RotateDirAnimator = false; 

    List<GameObject> blueprintImages = new List<GameObject>();

    float Rotation = 0f; 

    protected override void Awake()
    {
        blueprintImages = new List<GameObject>();
        BlueprintManager.BlueprintsChanged += BlueprintsChanged;
        BlueprintFees.PriceUpdated += PriceUpdated;
        base.Awake();
    }

    private void PriceUpdated(BlueprintType type, float fee)
    {
        if (type == BlueprintType)
        {
            MaintenanceFee = fee;
            print("new maintenance fee: " + MaintenanceFee);
        }
    }

    public override float EffectOnBalance()
    {
        return MaintenanceFee;
    }

    public override float EffectOnIncome(Vector2 pos)
    {
        return -1 * MaintenanceFee;
    }

    //this is becoming very deep inheritance
    public override void OnSelection()
    {
        base.OnSelection(); //definitely do that
        MaintenanceFee = BlueprintFees.GetMaintenanceFee(BlueprintType); //very nice, eh? 
    }

    public override string GetDescription()
    {
        string modification = "";

        if (LetterManager.UseGameplayChange(GameplayChange.RandomPrices))
        {
            modification += string.Format("\nStart price can be from {0} to {1}", BlueprintFees.GetMinFee(BlueprintType), BlueprintFees.GetMaxFee(BlueprintType));
        }
        if (LetterManager.UseGameplayChange(GameplayChange.IncrementPrice))
        {
            modification += string.Format("\nPrice increase: {0}", BlueprintFees.GetMaintenanceDelta(BlueprintType));
        }
        
        return base.GetDescription() + modification;
    }

    public override void UpdatePreview(bool visible, Vector2 mousePos)
    {
        if (visible && SummonPreview != null)
        {
            if (RotateDirAnimator)
            {
                if (SummonPreview.activeInHierarchy)
                {
                    SummonPreview.GetComponent<Animator>().enabled = true;
                    SummonPreview.GetComponent<DirectionalAnimator>().IdleDirection(Rotation);
                }
            } else
            {
                SummonPreview.transform.eulerAngles = new Vector3(0f, 0f, Rotation);
            }
        }
        base.UpdatePreview(visible, mousePos);
    }

    //what lol - that's how this works? 
    //it'd be a lot better if each blueprint took care of it's own graphic, and then when it's removed it automatically deletes itself 
    //we can do that without it being a gameobject as well
    private void BlueprintsChanged()
    {
        for (int i = blueprintImages.Count - 1; i >= 0; i--)
        {
            if (BlueprintManager.ShouldRemoveSummon(VectorRounder.RoundVectorToInt(blueprintImages[i].transform.position), BlueprintType))
            {
                GameObject g = blueprintImages[i];
                blueprintImages[i] = null;
                blueprintImages.RemoveAt(i);
                Destroy(g);
            }
        }
    }

    private void Update()
    {
        if (IsSelected)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Rotation -= 90f; 
            }
        }
    }

    public override bool CanUseWeapon(Vector2 mousePos)
    {
        if (BlueprintManager.IsPointTaken(VectorRounder.RoundVectorToInt(mousePos)))
        {
            return false; 
        }

        bool adjacentsPass = !HasRequiredAdjacents; //so, if we don't require adjacents, it's automatically true  

        List<Blueprint> adjacentSummons = BlueprintManager.GetAdjacentBlueprints(VectorRounder.RoundVectorToInt(mousePos));

        List<TileType> tiles = MapManager.GetAdjacentTiles(VectorRounder.RoundVectorToInt(mousePos));

        if (HasRequiredAdjacents)
        {
            foreach (Blueprint b in adjacentSummons)
            {
                if (RequiredAdjacents.Contains(b.BlueprintType))
                {
                    adjacentsPass = true;
                    break; 
                }
            }
            foreach (TileType t in tiles)
            {
                if (RequiredAdjacentTiles.Contains(t))
                {
                    adjacentsPass = true;
                    break;
                }
            }
        }

        foreach (Blueprint b in adjacentSummons)
        {
            if (BlacklistedAdjacents.Contains(b.BlueprintType))
            {
                adjacentsPass = false; 
            }
        }

        return base.CanUseWeapon(mousePos) && adjacentsPass;
    }

    public override float GetRecurringCost()
    {
        return MaintenanceFee;
    }

    public override void UseWeapon(Vector2 mousePos)
    {
        BlueprintManager.AddBlueprint(VectorRounder.RoundVectorToInt(mousePos), BlueprintType, Rotation, MaintenanceFee);
        GameObject b = Instantiate(Summon, VectorRounder.RoundVector(mousePos), Quaternion.Euler(new Vector3(0f, 0f, Rotation)));
        blueprintImages.Add(b);
        RangeVisualizer rv; 
        if (b.TryGetComponent<RangeVisualizer>(out rv))
        {
            rv.Hide();
        }
    }
}
