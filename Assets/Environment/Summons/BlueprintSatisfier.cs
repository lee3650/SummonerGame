using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class BlueprintSatisfier : PlayerWall, ILivingEntity, IRecurringCost, IControllableSummon
{
    [SerializeField] float Range = 25;
    [SerializeField] int MaxNumSummons = 6;

    [SerializeField] List<BlueprintType> Types; 
    [SerializeField] List<GameObject> Prefabs;
    [SerializeField] SpriteRenderer SpriteRenderer;

    [SerializeField] float MaintenanceFee = 5f; 

    private bool activated = true;

    List<BlueprintSummon> SummonedEntities = new List<BlueprintSummon>();

    protected override void Awake()
    {
        SummonedEntities = new List<BlueprintSummon>();
        TargetableEntitiesManager.AddTargetable(this);
        MySummon.SummonerSet += SummonerSet;
        MySummon.SummonWaveEnds += WaveEnds;
        base.Awake();
    }

    private void WaveEnds()
    {
        SetActivated();

        PlaceSummonsWhereAble();
    }

    void SetActivated()
    {
        if (MySummon.GetSummoner().TryReduceMana(MaintenanceFee))
        {
            Activated = true; 
        } else
        {
            Activated = false; 
        }
    }

    private void SummonerSet()
    {
        PlaceSummonsWhereAble();
        BlueprintManager.BlueprintsChanged += BlueprintsChanged;
        MySummon.SummonerSet -= SummonerSet;
    }

    public float GetRecurringCost()
    {
        return MaintenanceFee;
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

    public string GetStatString()
    {
        string stats = string.Format("Range: {0}\nMax Summons: {1}\nMaintenance Fee: {2}\nActivated: {3}", Range, MaxNumSummons, MaintenanceFee, activated);
        return stats; 
    }

    public bool CanBeSelected()
    {
        return true; 
    }

    private void PlaceSummonsWhereAble()
    {
        if (Activated)
        {
            PruneSummonedEntitiesList();

            int wallsToPlace = MaxNumSummons - SummonedEntities.Count;
            PlaceSummons(Types, Prefabs, wallsToPlace);
        }
    }

    private void PlaceSummons(List<BlueprintType> types, List<GameObject> prefabs, int numWalls)
    {
        List<Blueprint> prints = BlueprintManager.GetBlueprintsOfTypes(types);

        //I feel like this knows a bit too much - it could rely better on abstraction 

        for (int i = 0; i < numWalls; i++)
        {
            foreach (Blueprint p in prints)
            {
                if (!p.Satisfied)
                {
                    int index = types.IndexOf(p.BlueprintType);

                    p.Satisfied = true;

                    GameObject summoned = SummonEntity(prefabs[index], p.Point); 

                    HealthManager hm = summoned.GetComponent<HealthManager>();
                    SummonedEntities.Add(new BlueprintSummon(hm, p));
                    hm.OnDeath += PruneSummonedEntitiesList; //okay that's a little better. 

                    break; 
                }
            }
        }
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

    protected override void OnDeath()
    {
        TargetableEntitiesManager.RemoveTargetable(this);
        BlueprintManager.BlueprintsChanged -= BlueprintsChanged;
        MySummon.SummonWaveEnds -= WaveEnds;
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

    public Factions GetFaction()
    {
        return Factions.Player;
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
