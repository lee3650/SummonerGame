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

    [SerializeField] List<BlueprintType> Types; 
    [SerializeField] List<GameObject> Prefabs;
    [SerializeField] SpriteRenderer SpriteRenderer;

    [SerializeField] float MaintenanceFee = 5f; 

    private bool activated = true;

    List<BlueprintSummon> SummonedEntities = new List<BlueprintSummon>();

    [SerializeField] private float CalculatedMaintenanceFee; 

    public override void Init()
    {
        print("Init was called on blueprint satisfier!");
        SummonedEntities = new List<BlueprintSummon>();
        TargetableEntitiesManager.AddTargetable(this);
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

    private void CalculateMaintenanceFee(int totalSummons)
    {
        //this does require that all the summons we can place, have been placed, so. 
        CalculatedMaintenanceFee = MaintenanceFee * (float)totalSummons / (float)CalculateMaxSummons();
    }

    private void SummonerSet()
    {
        PlaceSummonsWhereAble();
        BlueprintManager.BlueprintsChanged += BlueprintsChanged;
        MySummon.SummonerSet -= SummonerSet;
    }

    public float GetRecurringCost()
    {
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
        foreach (BlueprintSummon bs in SummonedEntities)
        {
            if (BlueprintManager.ShouldRemoveSummon(bs.Point, bs.BlueprintType))
            {
                //that could get sketchy if we have weird death effects but whatever 
                
                bs.HealthManager.OnDeath -= PruneSummonedEntitiesList;
                //it's about to prune anyway, so this should be fine.
             
                bs.HealthManager.SubtractHealth(100000f);

                //we don't want to return here because technically more than one summon could be needed to be removed
            }
        }
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
        string stats = string.Format("Range: {0}\nMax Summons: {1}\nMaintenance Fee: {2}\nActivated: {3}", Range, CalculateMaxSummons(), MaintenanceFee, activated);
        return stats; 
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
            print(string.Format("Calculaed max summons: {0}, summoned entities.count: {1}", CalculateMaxSummons(), SummonedEntities.Count));
            print("Max summons: " + maxSummonsToPlace);
            List<Blueprint> validPrints = GetValidBlueprints(Types);
            print("Num of valid prints: " + validPrints.Count);
            int actualSummonsToPlace = Mathf.Min(maxSummonsToPlace, validPrints.Count);
            print("Actual summons to place: " + actualSummonsToPlace);
            print("-------------------");
            int summonsAfterPlacement = actualSummonsToPlace + SummonedEntities.Count;

            CalculateMaintenanceFee(summonsAfterPlacement);

            PlaceSummons(validPrints, Types, Prefabs, actualSummonsToPlace);
        }
    }

    int CalculateMaxSummons() //don't we really just need to calculate that once? The tile underneath can't change? 
    {
        if (prevNode.TileType == CapacityBuffTile)
        {
            return (int)(MaxNumSummons * capacityBuff);
        }

        return MaxNumSummons;
    }

    private void PlaceSummons(List<Blueprint> validPrints, List<BlueprintType> types, List<GameObject> prefabs, int actualSummons)
    {
        for (int i = 0; i < actualSummons; i++)
        {
            Blueprint p = validPrints[i];

            int index = types.IndexOf(p.BlueprintType);

            p.Satisfied = true;

            GameObject summoned = SummonEntity(prefabs[index], p.Point);

            HealthManager hm = summoned.GetComponent<HealthManager>();
            SummonedEntities.Add(new BlueprintSummon(hm, p));
            hm.OnDeath += PruneSummonedEntitiesList;

            break;
        }
    }

    List<Blueprint> GetValidBlueprints(List<BlueprintType> types)
    {
        List<Blueprint> prints = BlueprintManager.GetBlueprintsOfTypes(types);
        List<Blueprint> result = new List<Blueprint>();

        foreach (Blueprint p in prints)
        {
            if (!p.Satisfied && IsBlueprintInRange(p))
            {
                result.Add(p);
            }
        }

        return result; 
    }

    private bool IsBlueprintInRange(Blueprint p)
    {
        return Vector2.Distance(transform.position, p.Point) <= Range;
    }

    protected virtual GameObject SummonEntity(GameObject entity, Vector2 endPoint)
    {
        return SummonWeapon.SpawnSummon(entity, endPoint, MySummon.GetSummoner(), Quaternion.Euler(Vector3.zero));
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
