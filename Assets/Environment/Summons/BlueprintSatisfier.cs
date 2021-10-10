using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//I'm starting to think I'm doing something wrong with all these interfaces lol 
public class BlueprintSatisfier : PlayerWall, ILivingEntity, IRecurringCost, IControllableSummon, IRanged 
{
    [SerializeField] float Range = 25;
    [SerializeField] int MaxNumSummons = 6;

    [SerializeField] TileType CapacityBuffTile = TileType.Stone;
    const float capacityBuff = 1.5f;

    [SerializeField] List<BlueprintPrefabData> BlueprintData;  
    [SerializeField] SpriteRenderer SpriteRenderer;

    [SerializeField] float MaintenanceFee = 5f; 

    private bool activated = true;

    [SerializeField] bool AddToTargeting = true; 

    List<BlueprintSummon> SummonedEntities = new List<BlueprintSummon>();

    [SerializeField] private float CalculatedMaintenanceFee; 

    public override void Init()
    {
        print("Init was called on blueprint satisfier!");
        SummonedEntities = new List<BlueprintSummon>();
        if (AddToTargeting)
        {
            TargetableEntitiesManager.AddTargetable(this);
        }
        MySummon.SummonerSet += SummonerSet;
        MySummon.SummonWaveEnds += WaveEnds;
        base.Init();
    }

    private void WaveEnds()
    {
        SetActivated();
        PlaceSummonsWhereAble();
    }

    void SetActivated()
    {
        if (MySummon.GetSummoner().TryReduceMana(CalculatedMaintenanceFee))
        {
            Activated = true; 
        } else
        {
            Activated = false;
            RemoveAllSummons();
        }
    }

    public void CalculateMaintenanceFee()
    {
        float newFee = 0f;
        foreach (BlueprintSummon bs in SummonedEntities)
        {
            newFee += bs.Fee;
        }
        CalculatedMaintenanceFee = newFee; 
    }

    private void SummonerSet()
    {
        PlaceSummonsWhereAble();
        BlueprintManager.BlueprintsChanged += BlueprintsChanged;
        MySummon.SummonerSet -= SummonerSet;
    }

    public float GetRecurringCost()
    {
        CalculateMaintenanceFee();
        return CalculatedMaintenanceFee;
    }

    private void BlueprintsChanged()
    {
        if (WaveSpawner.IsCurrentWaveDefeated)
        {
            KillUnneededSummons();
            PlaceSummonsWhereAble();
        }
    }

    public void HandleCommand(PlayerCommand command)
    {
        switch (command)
        {
            case SellCommand sc:
                RemoveSummon();
                break;
            case UpgradeCommand uc:
                RemoveSummon();
                SummonWeapon.SpawnSummon(uc.UpgradePath.GetNextSummon(), transform.position, MySummon.GetSummoner(), transform.rotation);
                break; 
        }
    }

    public Transform GetTransform()
    {
        return transform; 
    }

    private void KillUnneededSummons()
    {
        List<BlueprintSummon> summons = FindSummonsToRemove();
        //CalculateMaintenanceFee(SummonedEntities.Count - summons.Count);

        foreach (BlueprintSummon bs in summons)
        {
            //that could get sketchy if we have weird death effects but whatever 

            bs.HealthManager.OnDeath -= PruneSummonedEntitiesList;
            //it's about to prune anyway, so this should be fine.

            bs.HealthManager.SubtractHealth(100000f);
        }

    }
    
    List<BlueprintSummon> FindSummonsToRemove()
    {
        List<BlueprintSummon> summons = new List<BlueprintSummon>();

        foreach (BlueprintSummon bs in SummonedEntities)
        {
            if (BlueprintManager.ShouldRemoveSummon(bs.Point, bs.BlueprintType) || !IsBlueprintInRange(bs.Point))
            {
                summons.Add(bs);
            }
        }

        return summons; 
    }

    public int SummonTier
    {
        get
        {
            return MySummon.SummonTier;
        }
    }

    public SummonType GetSummonType()
    {
        return MySummon.GetSummonType();
    }

    public string GetStatString()
    {
        string stats = string.Format("Max Summons: {1}\nSummons Types: {2}\nActivated: {3}", Range, CalculateMaxSummons(), GetSummonTypesString(), activated);
        return stats; 
    }

    protected string GetSummonTypesString()
    {
        string result = "";
        foreach (BlueprintPrefabData d in BlueprintData)
        {
            result += "\n" + d.ToString();
        }

        return result; 
    }

    public bool CanBeSelected()
    {
        return HealthManager.IsAlive(); 
    }

    private void PlaceSummonsWhereAble()
    {
        if (Activated)
        {
            PruneSummonedEntitiesList();

            int maxSummonsToPlace = CalculateMaxSummons() - SummonedEntities.Count;
            List<Blueprint> validPrints = GetValidBlueprints(BlueprintData);
            int actualSummonsToPlace = Mathf.Min(maxSummonsToPlace, validPrints.Count);
            
            List<BlueprintPrefabData> summonsToPlace = GetSummonsToPlace(validPrints, BlueprintData, actualSummonsToPlace);
            //CalculateMaintenanceFee(summonsToPlace);

            PlaceSummons(validPrints, summonsToPlace, actualSummonsToPlace);
        }
    }

    int CalculateMaxSummons() //don't we really just need to calculate that once? The tile underneath can't change? 
    {
        if (prevNode == null)
        {
            return MaxNumSummons;
        }

        if (prevNode.TileType == CapacityBuffTile)
        {
            return (int)(MaxNumSummons * capacityBuff);
        }

        return MaxNumSummons;
    }

    private List<BlueprintPrefabData> GetSummonsToPlace(List<Blueprint> validPrints, List<BlueprintPrefabData> data, int actualSummons)
    {
        List<BlueprintPrefabData> result = new List<BlueprintPrefabData>();

        for (int i = 0; i < actualSummons; i++)
        {
            result.Add(FindBlueprintDataFromType(validPrints[i].BlueprintType, data));
        }

        return result; 
    }

    private void PlaceSummons(List<Blueprint> validPrints, List<BlueprintPrefabData> summonsToPlace, int actualSummons)
    {
        for (int i = 0; i < actualSummons; i++)
        {
            Blueprint p = validPrints[i];
            BlueprintPrefabData d = summonsToPlace[i];

            p.Satisfied = true;

            GameObject summoned = SummonEntity(d.Prefab, p.Point, p.Rotation);

            HealthManager hm = summoned.GetComponent<HealthManager>();
            SummonedEntities.Add(new BlueprintSummon(hm, p, p.MaintenanceFee));
            hm.OnDeath += PruneSummonedEntitiesList;
        }
    }

    private BlueprintPrefabData FindBlueprintDataFromType(BlueprintType t, List<BlueprintPrefabData> data)
    {
        foreach (BlueprintPrefabData d in data)
        {
            if (d.BlueprintType == t)
            {
                return d;
            }
        }
        throw new System.Exception("Could not find blueprint data for type " + t.ToString());
    }

    List<Blueprint> GetValidBlueprints(List<BlueprintPrefabData> data)
    {
        List<BlueprintType> types = new List<BlueprintType>();
        foreach (BlueprintPrefabData d in data)
        {
            types.Add(d.BlueprintType);
        }

        List<Blueprint> prints = BlueprintManager.GetBlueprintsOfTypes(types);
        List<Blueprint> result = new List<Blueprint>();

        foreach (Blueprint p in prints)
        {
            if (!p.Satisfied && IsBlueprintInRange(p.Point))
            {
                result.Add(p);
            }
        }

        return result; 
    }

    private bool IsBlueprintInRange(Vector2 p)
    {
        return MySummon.GetSummoner().IsPointInSummonRange(p);
        //return Vector2.Distance(transform.position, p) <= Range;
    }
    
    protected virtual GameObject SummonEntity(GameObject entity, Vector2 endPoint, float rotation)
    {
        return SummonWeapon.SpawnSummon(entity, endPoint, MySummon.GetSummoner(), Quaternion.Euler(new Vector3(0f, 0f, rotation)));
    }

    private void PruneSummonedEntitiesList()
    {
        for (int i = SummonedEntities.Count - 1; i >= 0; i--)
        {
            if (SummonedEntities[i].IsAlive() == false)
            {
                //I also need to reset satisfied. Hm.
                SetSatisfied(SummonedEntities[i], false);
                SummonedEntities[i].HealthManager.OnDeath -= PruneSummonedEntitiesList;
                SummonedEntities.RemoveAt(i);
            }
        }
    }

    protected virtual void SetSatisfied(BlueprintSummon entity, bool val)
    {
        BlueprintManager.SetSatisfied(entity.Point, val);
    }

    protected void RemoveAllSummons()
    {
        foreach (BlueprintSummon bs in SummonedEntities)
        {
            bs.HealthManager.OnDeath -= PruneSummonedEntitiesList;
            bs.HealthManager.SubtractHealth(100000);
        }

        PruneSummonedEntitiesList();
    }

    protected override void OnDeath()
    {
        TargetableEntitiesManager.RemoveTargetable(this);
        BlueprintManager.BlueprintsChanged -= BlueprintsChanged;
        MySummon.SummonWaveEnds -= WaveEnds;
        RemoveAllSummons();
        base.OnDeath();
    }

    public bool Activated
    {
        get
        {
            return activated;
        }
        set
        {
            activated = value;
            float a = activated ? 1f : 0.5f; 
            SpriteRenderer.color = new Color(SpriteRenderer.color.r, SpriteRenderer.color.g, SpriteRenderer.color.b, a);
        }
    }

    public bool CanBeSold()
    {
        return AdjacentConnections.DoAdjacentTilesConnectToMiner(new Vector2Int((int)transform.position.x, (int)transform.position.y));
    }

    public Factions GetFaction()
    {
        return Factions.Player;
    }

    public float GetRange()
    {
        return Range;
    }

    public virtual bool IsCrossShaped()
    {
        return false;
    }

    public virtual float GetCrossDelta()
    {
        return 0f;
    }

    public void OnHit(IEntity hit)
    {
        throw new System.Exception("The wall generator got a hit?");
    }

    public List<Event> ModifyEventList(List<Event> unmodifiedList)
    {
        throw new System.Exception("Should not ask the wall generator to modify a list of events!");
    }
}
